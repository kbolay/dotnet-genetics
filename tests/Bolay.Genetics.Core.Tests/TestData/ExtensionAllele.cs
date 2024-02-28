using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Tests.TestData
{
    public class ExtensionAllele : Allele
    {
        public static readonly ExtensionAllele Steel = new ExtensionAllele(1, nameof(Steel), "E", "s");
        public static readonly ExtensionAllele Normal = new ExtensionAllele(2, nameof(Normal), "E");
        public static readonly ExtensionAllele Harlequin = new ExtensionAllele(3, nameof(Harlequin), "e", "j", DominanceEnum.Incomplete);
        public static readonly ExtensionAllele NonExtension = new ExtensionAllele(4, nameof(Steel), "e", dominance: DominanceEnum.Recessive);

        public ExtensionAllele(
            int ordinal, 
            string name, 
            string symbol, 
            string? genotypeSymbol = null,
            DominanceEnum dominance = DominanceEnum.Dominant)
        {
            this.Ordinal = ordinal;
            this.Name = name;
            this.Symbol = symbol;
            this.GenotypeSymbol = genotypeSymbol;
            this.Dominance = dominance;
        } // end method
    } // end method
} // end namespace