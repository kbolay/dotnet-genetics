using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity
{
    public class GenotypeRatio<TLocus>
        where TLocus : Locus, new()
    {
        public Genotype<TLocus> Genotype { get; set; }
        public float Ratio { get; set; }
    } // end class
} // end namespace