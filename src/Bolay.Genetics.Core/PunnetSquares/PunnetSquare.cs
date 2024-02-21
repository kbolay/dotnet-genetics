using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.PunnetSquares
{    
    public class PunnetSquare<TLocus>
        where TLocus : Locus, new()
    {
        public IEnumerable<GenotypeRatio<TLocus>> GetOffsprinGenotypes(
            Genotype<TLocus> paternalGenotype, 
            Genotype<TLocus> maternalGenotype, 
            params Allele<TLocus>[] locusAlleles)
        {
            var results = new List<GenotypeRatio<TLocus>>();

            var paternalAllelePositions = BuildPositions(paternalGenotype, locusAlleles);
            var maternalAllelePositions = BuildPositions(maternalGenotype, locusAlleles);

            var genePairs = paternalAllelePositions.SelectMany(paternalAllelePosition => 
                    maternalAllelePositions.SelectMany(maternalAllelePosition => 
                        paternalAllelePosition.SelectMany(paternalAllele => 
                            maternalAllelePosition.Select(maternalAllele => new Genotype<TLocus>()
                                {
                                    DominantAllele = paternalAllele.Ordinal <= maternalAllele.Ordinal ? paternalAllele : maternalAllele,
                                    OtherAllele = paternalAllele.Ordinal <= maternalAllele.Ordinal ? maternalAllele : paternalAllele
                                }))))
                    .ToList();

            results = genePairs.GroupBy(x => new { DominantOrdinal = x.DominantAllele.Ordinal, OtherOrdinal = x.OtherAllele.Ordinal})
                .Select(x => new GenotypeRatio<TLocus>() 
                    { 
                        Pair = x.First(),
                        Ratio = (float)x.Count() / (float)genePairs.Count
                    })
                .ToList();

            return results;
        } // end method

        public IEnumerable<ParentGenotypeRatio<TLocus>> GetParentGenotypes(
            IEnumerable<Genotype<TLocus>> offspringGenotypes,
            Genotype<TLocus>? paternalGenotype = null,
            Genotype<TLocus>? maternalGenotype = null)
        {
            throw new NotImplementedException();
        } // end method

        protected IEnumerable<IEnumerable<Allele<TLocus>>> BuildPositions(Genotype<TLocus> parentGenotype, params Allele<TLocus>[] locusAlleles)
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
    } // end class
} // end namespace