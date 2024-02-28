using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity
{
    public class IndividualGenotypeRatio<TAllele, TLocus, TId> : Individual<TId>
        where TAllele : Allele
        where TLocus : Locus<TAllele>, new()
    {
        public IEnumerable<GenotypeRatio<TAllele, TLocus>> GenotypeRatios { get; set; }
    } // end class

    public class IndividualGenotypeRatio<TAllele, TLocus> : IndividualGenotypeRatio<TAllele, TLocus, Guid>
        where TAllele : Allele
        where TLocus : Locus<TAllele>, new()
    {

    } // end class
} // end namespace