namespace Bolay.Genetics.Core.Models
{
    /// <summary>
    /// An allele is a gene or the synonym of a gene that can fit in the same place on a chromosome.
    /// Different alleles can have different impacts on the phenotypes produced by this gene.
    /// </summary>
    public class Allele<TLocus>
        where TLocus : Locus, new()
    {
        /// <summary>
        /// Gets or sets the locus on which the allele can exist.
        /// </summary>
        public TLocus Locus { get; set; } = new TLocus();

        /// <summary>
        /// Gets or sets ordinal value of this allele to be compared against the other alleles of the same gene locus.
        /// This is primarily for the purpose of dominance.
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// Gets or sets the name of the allele.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the symbol of the allele.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the genotype symbol. Often displayed in superscript. 
        /// </summary>
        public string GenotypeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the phenotype symbol. Often displayed in subscript.
        /// </summary>
        public string PhenotypeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the dominance of this allele in relation to alleles with higher ordinal values.
        /// </summary>
        public DominanceEnum Dominance { get; set; } = DominanceEnum.Dominant;

        /// <summary>
        /// Gets or sets if this allele is the wild type gene.
        /// </summary>
        public bool IsWildType { get; set; }

        /// <summary>
        /// Gets or sets the base pairs that make this allele.
        /// </summary>
        public IEnumerable<BasePair>? BasePairs { get; set; }
    } // end class
} // end namespace