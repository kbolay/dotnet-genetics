using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity
{
    public class IndividualGenotype<TAllele, TLocus, TId> : Individual<TId>
        where TAllele : Allele
        where TLocus : Locus<TAllele>, new()
    {
        public Genotype<TAllele, TLocus> Genotype { get; set; }
    } // end class

    public class IndividualGenotype<TAllele, TLocus> : IndividualGenotype<TAllele, TLocus, Guid>
        where TAllele : Allele
        where TLocus : Locus<TAllele>, new()
    {} // end class
} // end namespace