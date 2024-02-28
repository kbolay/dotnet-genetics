using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity
{
    public class GenotypeRatio<TAllele, TLocus>
        where TAllele : Allele
        where TLocus : Locus<TAllele>, new()
    {
        public Genotype<TAllele, TLocus> Genotype { get; set; }
        public float Ratio { get; set; }
    } // end class
} // end namespace