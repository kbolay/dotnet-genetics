using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.PunnetSquares
{
    public class GenotypeRatio<TLocus>
        where TLocus : Locus, new()
    {
        public Genotype<TLocus> Pair { get; set; }
        public float Ratio { get; set; }
    } // end class
} // end namespace