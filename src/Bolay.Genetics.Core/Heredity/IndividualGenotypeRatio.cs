using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity
{
    public class IndividualGenotypeRatio<TLocus, TId> : Individual<TId>
        where TLocus : Locus, new()
    {
        public IEnumerable<GenotypeRatio<TLocus>> GenotypeRatios { get; set; }
    } // end class

    public class IndividualGenotypeRatio<TLocus> : IndividualGenotypeRatio<TLocus, Guid>
        where TLocus : Locus, new()
    {

    } // end class
} // end namespace