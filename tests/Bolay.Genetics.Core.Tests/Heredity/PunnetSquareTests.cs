using Bolay.Genetics.Core.Models;
using Bolay.Genetics.Core.Heredity;
using Xunit;

namespace Bolay.Genetics.Core.Tests.Heredity
{
    public class PunnetSquareTests
    {
        private static FirstAllele _firstAllele = new FirstAllele();
        private static SecondAllele _secondAllele = new SecondAllele();

        private static List<Allele<TestLocus>> _availableAlleles = new List<Allele<TestLocus>>()
        {
            _firstAllele,
            _secondAllele
        };

        private PunnetSquare<TestLocus, Guid> _punnetSquare;

        public PunnetSquareTests()
        {
            _punnetSquare = new PunnetSquare<TestLocus, Guid>();
        } // end method

        [Fact]
        public void Execute_AA_AA()
        {
            var paternalPair = new Genotype<TestLocus>(_firstAllele, _firstAllele);
            var maternalPair = new Genotype<TestLocus>(_firstAllele, _firstAllele);
            var results = _punnetSquare.GetOffsprinGenotypes(paternalPair, maternalPair, _availableAlleles.ToArray());

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(1, results.Count());
            var firstResult = results.First();
            Assert.Equal(1, firstResult.Ratio);
            Assert.Equal(_firstAllele, firstResult.Genotype.DominantAllele);
            Assert.Equal(_firstAllele, firstResult.Genotype.OtherAllele);
        } // end method

        [Fact]
        public void Execute_A_A_()
        {
            var paternalPair = new Genotype<TestLocus>(_firstAllele, null);
            var maternalPair = new Genotype<TestLocus>(_firstAllele, null);
            var results = _punnetSquare.GetOffsprinGenotypes(paternalPair, maternalPair, _availableAlleles.ToArray());

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(3, results.Count());
            var firstResult = results.First();
            Assert.Equal((float)4/(float)9, firstResult.Ratio);
            Assert.Equal(_firstAllele, firstResult.Genotype.DominantAllele);
            Assert.Equal(_firstAllele, firstResult.Genotype.OtherAllele);

            var secondResult = results.ElementAt(1);
            Assert.Equal((float)4/(float)9, secondResult.Ratio);
            Assert.Equal(_firstAllele, secondResult.Genotype.DominantAllele);
            Assert.Equal(_secondAllele, secondResult.Genotype.OtherAllele);

            var thirdResult = results.Last();
            Assert.Equal((float)1/(float)9, thirdResult.Ratio);
            Assert.Equal(_secondAllele, thirdResult.Genotype.DominantAllele);
            Assert.Equal(_secondAllele, thirdResult.Genotype.OtherAllele);
        } // end method

        [Fact]
        public void Execute_Unknown()
        {
            var paternalPair = new Genotype<TestLocus>();
            var maternalPair = new Genotype<TestLocus>();
            var results = _punnetSquare.GetOffsprinGenotypes(paternalPair, maternalPair, _availableAlleles.ToArray());

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(3, results.Count());
            var firstResult = results.First();
            Assert.Equal(.25, firstResult.Ratio);
            Assert.Equal(_firstAllele, firstResult.Genotype.DominantAllele);
            Assert.Equal(_firstAllele, firstResult.Genotype.OtherAllele);

            var secondResult = results.ElementAt(1);
            Assert.Equal(.5, secondResult.Ratio);
            Assert.Equal(_firstAllele, secondResult.Genotype.DominantAllele);
            Assert.Equal(_secondAllele, secondResult.Genotype.OtherAllele);

            var thirdResult = results.Last();
            Assert.Equal(.25, thirdResult.Ratio);
            Assert.Equal(_secondAllele, thirdResult.Genotype.DominantAllele);
            Assert.Equal(_secondAllele, thirdResult.Genotype.OtherAllele);
        } // end method
    } // end class
} // end namespace