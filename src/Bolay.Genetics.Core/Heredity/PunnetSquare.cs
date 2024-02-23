using System.Runtime.CompilerServices;
using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity
{    
    public class PunnetSquare<TLocus, TId>
        where TLocus : Locus, new()
    {
        public IEnumerable<GenotypeRatio<TLocus>> GetOffsprinGenotypes(
            Genotype<TLocus> paternalGenotype, 
            Genotype<TLocus> maternalGenotype, 
            params Allele<TLocus>[] locusAlleles)
        {
            var paternalAlleleSets = BuildAlleleSets(paternalGenotype, locusAlleles);
            var maternalAlleleSets = BuildAlleleSets(maternalGenotype, locusAlleles);

            var potentialOffspringGenotypes = CombineAlleleSets(paternalAlleleSets, maternalAlleleSets);

            var genotypeRatios = BuildGenotypeRatios(potentialOffspringGenotypes);

            return genotypeRatios;
        } // end method

        /// <summary>
        /// Build the potential alleles of a given genotype.
        /// If one of the genes is unknown then a genotype is created for each equal or higher ordinal (lower dominance) allele.
        /// </summary>
        /// <param name="parentGenotype"></param>
        /// <param name="locusAlleles"></param>
        /// <returns></returns>
        protected IEnumerable<IEnumerable<Allele<TLocus>>> BuildAlleleSets(Genotype<TLocus> parentGenotype, params Allele<TLocus>[] locusAlleles)
        {
            var result = new List<List<Allele<TLocus>>>();
            var firstAlleles = new List<Allele<TLocus>>();
            if(parentGenotype.DominantAllele != null)
            {
                firstAlleles.Add(parentGenotype.DominantAllele);
            }
            else
            {
                firstAlleles.AddRange(locusAlleles);
            } // end if

            result.Add(firstAlleles);

            var secondAlleles = new List<Allele<TLocus>>();
            if(parentGenotype.OtherAllele != null)
            {
                secondAlleles.Add(parentGenotype.OtherAllele);
            }
            else
            {
                if(parentGenotype.DominantAllele != null)
                {
                    secondAlleles.AddRange(locusAlleles.Where(x => x.Ordinal >= parentGenotype.DominantAllele.Ordinal));
                } 
                else
                {
                    secondAlleles.AddRange(locusAlleles);
                } // end if
            } // end if
            result.Add(secondAlleles);

            return result;
        } // end method

        /// <summary>
        /// Run punnet square (cross join) for each set of genotypes of each parent.
        /// </summary>
        /// <param name="paternalSets"></param>
        /// <param name="maternalSets"></param>
        /// <returns></returns>
        protected IEnumerable<Genotype<TLocus>> CombineAlleleSets(
            IEnumerable<IEnumerable<Allele<TLocus>>> paternalSets, 
            IEnumerable<IEnumerable<Allele<TLocus>>> maternalSets)
        {
            return paternalSets.SelectMany(paternalAlleleSet => 
                    maternalSets.SelectMany(maternalAlleleSet => 
                        paternalAlleleSet.SelectMany(paternalAllele => 
                            maternalAlleleSet.Select(maternalAllele => new Genotype<TLocus>()
                                {
                                    DominantAllele = paternalAllele.Ordinal <= maternalAllele.Ordinal ? paternalAllele : maternalAllele,
                                    OtherAllele = paternalAllele.Ordinal <= maternalAllele.Ordinal ? maternalAllele : paternalAllele
                                }))))
                    .ToList();
        } // end method

        /// <summary>
        /// Takes a set of genotypes and returns the distinct ratios of each.
        /// </summary>
        /// <param name="genotypes"></param>
        /// <returns></returns>
        protected IEnumerable<GenotypeRatio<TLocus>> BuildGenotypeRatios(IEnumerable<Genotype<TLocus>> genotypes)
        {
            return genotypes
                .GroupBy(x => new { DominantOrdinal = x.DominantAllele?.Ordinal, OtherOrdinal = x.OtherAllele?.Ordinal})
                .Select(x => new GenotypeRatio<TLocus>() 
                    { 
                        Genotype = x.First(),
                        Ratio = (float)x.Count() / (float)genotypes.Count()
                    })
                .ToList();
        } // end method
    } // end class
} // end namespace