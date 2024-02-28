using Bolay.Genetics.Core.Heredity;
using Xunit;
using Bolay.Genetics.Core.TestData;
using Moq;
using Microsoft.Extensions.Logging;
using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Tests.Heredity
{
    public class PunnetSquareTests
    {
        private static FirstAllele _firstAllele = new FirstAllele();
        private static SecondAllele _secondAllele = new SecondAllele();
        private static ThirdAllele _thirdAllele = new ThirdAllele();

        private Mock<ILogger<PunnetSquare<TestAllele, TestLocus>>> _logger;

        private PunnetSquare<TestAllele, TestLocus> _punnetSquare;

        public PunnetSquareTests()
        {
            _logger = new Mock<ILogger<PunnetSquare<TestAllele, TestLocus>>>();
            _punnetSquare = new PunnetSquare<TestAllele, TestLocus>(_logger.Object);
        } // end method

        [Fact]
        public void Execute_AA_AA()
        {
            var paternalPair = new Genotype<TestAllele, TestLocus>(_firstAllele, _firstAllele);
            var maternalPair = new Genotype<TestAllele, TestLocus>(_firstAllele, _firstAllele);
            var results = _punnetSquare.GetOffsprinGenotypes(paternalPair, maternalPair);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(1, results.Count());
            var firstResult = results.First();
            Assert.Equal(1, firstResult.Ratio);
            Assert.Equal(_firstAllele, firstResult.Genotype.DominantAllele);
            Assert.Equal(_firstAllele, firstResult.Genotype.OtherAllele);
        } // end method

        [Fact]
        public void Execute_A_A()
        {
            var paternalPair = new Genotype<TestAllele, TestLocus>(_firstAllele, null);
            var maternalPair = new Genotype<TestAllele, TestLocus>(_firstAllele, null);
            var results = _punnetSquare.GetOffsprinGenotypes(paternalPair, maternalPair);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(6, results.Count());
            var firstResult = results.First();
            Assert.Equal((float)16/(float)36, firstResult.Ratio);
            Assert.Equal(_firstAllele, firstResult.Genotype.DominantAllele);
            Assert.Equal(_firstAllele, firstResult.Genotype.OtherAllele);

            var secondResult = results.ElementAt(1);
            Assert.Equal((float)8/(float)36, secondResult.Ratio);
            Assert.Equal(_firstAllele, secondResult.Genotype.DominantAllele);
            Assert.Equal(_secondAllele.ToString(), secondResult.Genotype.OtherAllele.ToString());

            var thirdResult = results.ElementAt(2);
            Assert.Equal((float)8/(float)36, thirdResult.Ratio);
            Assert.Equal(_firstAllele, thirdResult.Genotype.DominantAllele);
            Assert.Equal(_thirdAllele.ToString(), thirdResult.Genotype.OtherAllele.ToString());

            var fourthResult = results.ElementAt(3);
            Assert.Equal((float)1/(float)36, fourthResult.Ratio);
            Assert.Equal(_secondAllele.ToString(), fourthResult.Genotype.DominantAllele.ToString());
            Assert.Equal(_secondAllele.ToString(), fourthResult.Genotype.OtherAllele.ToString());

            var fifthResult = results.ElementAt(4);
            Assert.Equal((float)2/(float)36, fifthResult.Ratio);
            Assert.Equal(_secondAllele.ToString(), fifthResult.Genotype.DominantAllele.ToString());
            Assert.Equal(_thirdAllele.ToString(), fifthResult.Genotype.OtherAllele.ToString());

            var sixthResult = results.Last();
            Assert.Equal((float)1/(float)36, sixthResult.Ratio);
            Assert.Equal(_thirdAllele.ToString(), sixthResult.Genotype.DominantAllele.ToString());
            Assert.Equal(_thirdAllele.ToString(), sixthResult.Genotype.OtherAllele.ToString());
        } // end method

        [Fact]
        public void Execute_Unknown()
        {
            var paternalPair = new Genotype<TestAllele, TestLocus>();
            var maternalPair = new Genotype<TestAllele, TestLocus>();
            var results = _punnetSquare.GetOffsprinGenotypes(paternalPair, maternalPair);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(6, results.Count());
        } // end method
    } // end class
} // end namespace