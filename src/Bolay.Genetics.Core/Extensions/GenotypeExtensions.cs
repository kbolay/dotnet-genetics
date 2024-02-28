using Bolay.Genetics.Core.Heredity;
using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Extensions
{
    public static class GenotypeExtensions
    {
        public static IndividualGenotype<TAllele, TLocus, TId> BuildIndividual<TAllele, TLocus, TId>(
                this Genotype<TAllele, TLocus> genotype,
                Func<TId> idProvider,
                TId? paternalId = default(TId?),
                TId? maternalId = default(TId?))
            where TAllele : Allele
            where TLocus : Locus<TAllele>, new()
        {
            return new IndividualGenotype<TAllele, TLocus, TId>()
            {
                Id = idProvider(),
                Genotype = genotype,
                PaternalId = paternalId,
                MaternalId = maternalId
            };
        } // end method

        public static IEnumerable<Genotype<TAllele, TLocus>> BuildPotentialGenotypes<TAllele, TLocus>(
                this Genotype<TAllele, TLocus> genotype)
            where TAllele : Allele
            where TLocus : Locus<TAllele>, new()
        {
            var locus = new TLocus();
            
            var result = new List<Genotype<TAllele, TLocus>>();

            if(genotype.DominantAllele == null)
            {
                result = locus.Alleles.SelectMany(dominant => 
                    locus.Alleles
                        .Where(other => other.Ordinal >= dominant.Ordinal)
                        .Select(other => new Genotype<TAllele, TLocus>(dominant, other)))
                    .ToList();
            }
            else if(genotype.OtherAllele == null)
            {
                // dominant is known, need to fill in the other allele.
                result = locus.Alleles
                    .Where(other => other.Ordinal >= genotype.DominantAllele.Ordinal)
                    .Select(other => new Genotype<TAllele, TLocus>(genotype.DominantAllele, other))
                    .ToList();
            }
            else
            {
                result.Add(genotype);
            } // end if

            return result;
        } // end method
    } // end class
} // end namespace