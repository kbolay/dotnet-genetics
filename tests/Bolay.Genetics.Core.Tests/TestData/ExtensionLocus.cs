using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Tests.TestData
{
    public class ExtensionLocus : Locus<ExtensionAllele>
    {        
        public ExtensionLocus()
        {
            this.Id = Guid.NewGuid();
            this.Symbol = "E";
        }

        public override IEnumerable<ExtensionAllele> Alleles => new List<ExtensionAllele>()
            {
                ExtensionAllele.Steel,
                ExtensionAllele.Normal,
                ExtensionAllele.Harlequin,
                ExtensionAllele.NonExtension
            };
    } // end class
} // end namespace