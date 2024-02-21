namespace Bolay.Genetics.Core.Models
{
    /// <summary>
    /// A pair of alleles located at a specific locus.
    /// </summary>
    /// <typeparam name="TLocus"></typeparam>
    public class Genotype<TLocus>
        where TLocus : Locus, new()
    {
        /// <summary>
        /// Gets or sets the more dominant allele.
        /// </summary>
        public Allele<TLocus>? DominantAllele { get; set; }

        /// <summary>
        /// Gets or sets the less dominant allele. Can be the same as the first allele, different, or unknown.
        /// </summary>
        public Allele<TLocus>? OtherAllele { get; set; }

        public Genotype() { } // end method

        public Genotype(Allele<TLocus> dominantAllele, Allele<TLocus>? otherAllele)
        {
            DominantAllele = dominantAllele ?? throw new ArgumentNullException(nameof(dominantAllele));
            OtherAllele = otherAllele;

            if(otherAllele != null && otherAllele.Ordinal < dominantAllele.Ordinal)
            {
                throw new ArgumentException("OtherAllele must be of equal or lower dominance.");
            } // end if
        } // end method

        public override bool Equals(object? obj)
        {
            var result = false;

            if(obj != null && obj is Genotype<TLocus>)
            {
                var otherGenePair = (Genotype<TLocus>)obj;

                if(otherGenePair != null
                    && this.DominantAllele?.Ordinal == otherGenePair.DominantAllele?.Ordinal
                    && this.OtherAllele?.Ordinal == otherGenePair.OtherAllele?.Ordinal)
                {
                    result = true;
                } // end if
            } // end if

            return result;
        } // end method
    } // end class
} // end namespace