using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.TestData
{
    public class FirstAllele : TestAllele
    {
        public FirstAllele()
        {
            this.Ordinal = 0;
            this.Name = "First";
            this.Symbol = "A";
            this.Dominance = DominanceEnum.Dominant;
        } // end method
    } // end class

} // end namespace