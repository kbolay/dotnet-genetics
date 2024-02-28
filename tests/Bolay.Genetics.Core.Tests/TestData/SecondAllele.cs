using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.TestData
{
    public class SecondAllele : TestAllele
    {
        public SecondAllele()
        {
            this.Ordinal = 1;
            this.Name = "Second";
            this.Symbol = "a";
            this.GenotypeSymbol = "t";
            this.Dominance = DominanceEnum.Dominant;
        } // end method
    } // end class

} // end namespace