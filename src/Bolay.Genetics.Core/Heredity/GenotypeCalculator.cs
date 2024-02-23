using System.Security.Cryptography.X509Certificates;
using Bolay.Genetics.Core.Heredity.Interfaces;
using Bolay.Genetics.Core.Models;
using Microsoft.Extensions.Logging;

namespace Bolay.Genetics.Core.Heredity
{
    public class GenotypeCalculator<TLocus, TAllele, TId> : IGenotypeCalculator<TLocus, TId>
        where TLocus : Locus, new()
        where TAllele : Allele<TLocus>
    {
        protected readonly IGenotypeRepository<TLocus, TId> _genotypeRepository;
        protected readonly IAlleleRepository<TLocus, TAllele> _alleleRepository;
        protected readonly PunnetSquare<TLocus, TId> _punnetSquare;
        protected readonly ILogger _logger;

        public GenotypeCalculator(
            IGenotypeRepository<TLocus, TId> genotypeRepository,
            IAlleleRepository<TLocus, TAllele> alleleRepository,
            PunnetSquare<TLocus, TId> punnetSquare,
            ILogger<GenotypeCalculator<TLocus, TAllele, TId>> logger)
        {
            _genotypeRepository = genotypeRepository ?? throw new ArgumentNullException(nameof(genotypeRepository));
            _alleleRepository = alleleRepository ?? throw new ArgumentNullException(nameof(alleleRepository));
            _punnetSquare = punnetSquare ?? throw new ArgumentNullException(nameof(punnetSquare));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        } // end method

        public virtual async Task<IEnumerable<Genotype<TLocus>>> CalculateGenotypesAsync(TId individualId, CancellationToken token = default)
        {
            var result = new List<Genotype<TLocus>>();

            var individualGenotype = await _genotypeRepository.GetAsync(individualId, token).ConfigureAwait(false);
            var knownGenotype = individualGenotype.Genotype;

            var locusAlleles = await _alleleRepository.GetAsync(token).ConfigureAwait(false);
            var recessiveAllele = locusAlleles.OrderBy(x => x.Ordinal).Last();

            if(knownGenotype.DominantAllele != null)
            {
                if(knownGenotype.DominantAllele.Ordinal == recessiveAllele.Ordinal)
                {
                    result.Add(new Genotype<TLocus>()
                    {
                        DominantAllele = recessiveAllele,
                        OtherAllele = recessiveAllele
                    });
                }
                else
                {
                    result = locusAlleles
                        .Where(x => x.Ordinal >= knownGenotype.DominantAllele.Ordinal)
                        .Select(allele => new Genotype<TLocus>()
                        {
                            DominantAllele = knownGenotype.DominantAllele,
                            OtherAllele = allele
                        })
                        .ToList();
                } // end if
            }
            else
            {
                result = locusAlleles.SelectMany(firstAllele => 
                    locusAlleles.Where(secondAllele => firstAllele.Ordinal <= secondAllele.Ordinal)
                        .Select(secondAllele => new Genotype<TLocus>()
                        {
                            DominantAllele = firstAllele,
                            OtherAllele = secondAllele                            
                        }))
                    .ToList();
            } // end if

            // get the potential genotype ratios from the parents of this individual
            var genotypeRatiosFromParents = await GetParentsGenotypeRatiosAsync(individualGenotype, token).ConfigureAwait(false);

            // restrict my potential viable genotypes down to those from the parent.
            if(genotypeRatiosFromParents != null && genotypeRatiosFromParents.Any())
            {
                result = result
                    .Where(viableGenotype => 
                        genotypeRatiosFromParents.Any(x => 
                            x.Genotype.DominantAllele.Ordinal == viableGenotype.DominantAllele.Ordinal
                            && x.Genotype.OtherAllele.Ordinal == viableGenotype.OtherAllele.Ordinal))
                    .ToList();
            } // end if

            if(result.Count() > 1)
            {
                var childrenGenotypeRatios = await GetPotentialGenotypesFromOffspringAsync(individualGenotype, token).ConfigureAwait(false);

                if(childrenGenotypeRatios != null && childrenGenotypeRatios.Any())
                {
                    result = result
                        .Where(viableGenotype => childrenGenotypeRatios.Any(x => 
                            viableGenotype.DominantAllele.Ordinal == x.DominantAllele.Ordinal
                            && viableGenotype.OtherAllele.Ordinal == x.OtherAllele.Ordinal))
                        .ToList();
                } // end if
            } // end if

            return result;
        } // end method
    
        protected virtual async Task<IEnumerable<GenotypeRatio<TLocus>>> GetParentsGenotypeRatiosAsync(
            Individual<TId> individual, 
            CancellationToken token = default)
        {
            var availableAlleles = await _alleleRepository.GetAsync(token).ConfigureAwait(false);

            var paternalGenotype = new Genotype<TLocus>();
            var materalGenotype = new Genotype<TLocus>();
            
            if(individual.PaternalId != null)
            {
                var father = await _genotypeRepository.GetAsync(individual.PaternalId, token).ConfigureAwait(false);
                if(father != null)
                {
                    paternalGenotype = father.Genotype;
                } // end if
            } // end if
            
            if(individual.MaternalId != null)
            {
                var mother = await _genotypeRepository.GetAsync(individual.MaternalId, token).ConfigureAwait(false);
                if(mother != null)
                {
                    materalGenotype = mother.Genotype;
                } // end if
            } // end if

            return _punnetSquare.GetOffsprinGenotypes(paternalGenotype, materalGenotype, availableAlleles.ToArray());
        } // end method
    
        protected virtual async Task<IEnumerable<Genotype<TLocus>>> GetPotentialGenotypesFromOffspringAsync(
            IndividualGenotype<TLocus, TId> individual,
            CancellationToken token = default)
        {
            var result = new List<Genotype<TLocus>>();

            var locusAlleles = await _alleleRepository.GetAsync(token).ConfigureAwait(false);

            // Get all the offspring of the individual
            var allOffspring = await _genotypeRepository.GetOffspringAsync(individual.Id, token).ConfigureAwait(false);

            // are there any offspring?
            if(allOffspring != null && allOffspring.Any())
            {
                // group the offspring by both parents
                var offspringParentGroups = allOffspring
                    .Where(x => x.MaternalId != null && x.PaternalId != null) // not sure if this is necessary
                    .GroupBy(x => new { PaternalId = x.PaternalId, MaternalId = x.MaternalId });

                foreach(var offspringParentGroup in offspringParentGroups)
                {
                    var potentialGenotypes = await BuildParentGenotypesFromSiblingSetsAsync(individual, offspringParentGroup, locusAlleles, token).ConfigureAwait(false);
                    result.AddRange(potentialGenotypes);                    
                } // end foreach
            } // end if

            return result
                .GroupBy(x => new { DominantOrdinal = x.DominantAllele.Ordinal, OtherOrdinal = x.OtherAllele?.Ordinal })
                .Select(x => x.First())
                .ToList();
        } // end method

        protected async Task<IEnumerable<Genotype<TLocus>>> BuildParentGenotypesFromSiblingSetsAsync(
            IndividualGenotype<TLocus, TId>? individual,
            IEnumerable<IndividualGenotype<TLocus, TId>> siblings,
            IEnumerable<TAllele> locusAlleles,
            CancellationToken token = default)
        {
            var uniqueOffspringGenotypes = siblings
                .Where(x => x.Genotype.DominantAllele != null)
                .GroupBy(x => new { DominantOrdinal = x.Genotype.DominantAllele.Ordinal, OtherOrdinal = x.Genotype.OtherAllele?.Ordinal })
                .Select(x => x.First().Genotype)
                .OrderBy(x => x.DominantAllele.Ordinal)
                .ToList();

            var dominantGroups = uniqueOffspringGenotypes.GroupBy(x => x.DominantAllele.Ordinal);
            var mostDominantOffspringAllele = uniqueOffspringGenotypes.First().DominantAllele;
            var potentialDominantAlleles = locusAlleles.Where(x => x.Ordinal >= mostDominantOffspringAllele.Ordinal);

            var leastDominantOffspringAllele = uniqueOffspringGenotypes.Last().DominantAllele;
            var potentialOtherAlleles = locusAlleles.Where(x => x.Ordinal >= leastDominantOffspringAllele.Ordinal);

            var recessiveAllele = locusAlleles.OrderBy(x => x.Ordinal).Last();
            if(leastDominantOffspringAllele.Ordinal == recessiveAllele.Ordinal)
            {
                potentialOtherAlleles = potentialDominantAlleles.Where(x => x.Ordinal == recessiveAllele.Ordinal);
            } // end if        
            
            if(individual.Genotype.DominantAllele == null && dominantGroups.Count() > 2)                
            {
                var otherParentId = siblings.Any(x => individual.Id.Equals(x.PaternalId)) ? siblings.First().MaternalId : siblings.First().PaternalId;
                var otherParent = await _genotypeRepository.GetAsync(otherParentId, token).ConfigureAwait(false);

                if(otherParent.Genotype.DominantAllele != null)
                {
                    var secondDominantAllele = dominantGroups.Skip(1).First().First().DominantAllele;
                    if(otherParent.Genotype.DominantAllele.Ordinal == mostDominantOffspringAllele.Ordinal)
                    {
                        // the most dominant allele comes from the individual we are trying to figure out                        
                        potentialDominantAlleles = potentialDominantAlleles.Where(x => x.Ordinal == secondDominantAllele.Ordinal);
                        potentialOtherAlleles = potentialOtherAlleles.Where(x => x.Ordinal > secondDominantAllele.Ordinal);
                    }
                    else if (otherParent.Genotype.DominantAllele.Ordinal == secondDominantAllele.Ordinal)
                    {
                        potentialDominantAlleles = potentialDominantAlleles.Where(x => x.Ordinal == mostDominantOffspringAllele.Ordinal);
                        potentialOtherAlleles = potentialDominantAlleles.Where(x => x.Ordinal >= leastDominantOffspringAllele.Ordinal);
                    } // end if                    
                } // end if
            }
            else if(individual.Genotype.DominantAllele.Ordinal >= leastDominantOffspringAllele.Ordinal)
            {                
                potentialDominantAlleles = potentialDominantAlleles.Where(x => x.Ordinal >= individual.Genotype.DominantAllele.Ordinal);
                potentialOtherAlleles = potentialOtherAlleles.Where(x => x.Ordinal >= individual.Genotype.DominantAllele.Ordinal);
            } // end if                      

            return potentialDominantAlleles
                .SelectMany(dominant => 
                    potentialOtherAlleles.Select(other => new Genotype<TLocus>()
                    {
                        DominantAllele = dominant,
                        OtherAllele = other
                    }));
        } // end method
    } // end class
} // end namespace