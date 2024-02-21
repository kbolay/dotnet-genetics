using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Tests.PunnetSquares
{
    public class TestLocus : Locus
    {
        public TestLocus() 
        {
            this.Symbol = "A";
            this.Name = "Test";
            this.Description = "test";
        } // end method
    } // end class

    public class FirstAllele : Allele<TestLocus>
    {
        public FirstAllele()
        {
            this.Ordinal = 0;
            this.Name = "First";
            this.Symbol = "A";
            this.Dominance = DominanceEnum.Dominant;
        } // end method
    } // end class

    public class SecondAllele : Allele<TestLocus>
    {
        public SecondAllele()
        {
            this.Ordinal = 1;
            this.Name = "Second";
            this.Symbol = "a";
            this.Dominance = DominanceEnum.Recessive;
        } // end method
    } // end class

} // end namespace