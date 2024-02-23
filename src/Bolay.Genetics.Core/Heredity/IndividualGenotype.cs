using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity
{
    public class IndividualGenotype<TLocus, TId> : Individual<TId>
        where TLocus : Locus, new()
    {
        public Genotype<TLocus> Genotype { get; set; }
    } // end class

    public class IndividualGenotype<TLocus> : IndividualGenotype<TLocus, Guid> 
        where TLocus : Locus, new()
    {} // end class
} // end namespace