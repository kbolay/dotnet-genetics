using Bolay.Genetics.Core.Heredity;
using Bolay.Genetics.Core.Heredity.Interfaces;
using Bolay.Genetics.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Bolay.Genetics.Core.Tests.Heredity
{
    public class GenotypeCalculatorTests
    {
        private Mock<IAlleleRepository<TestLocus, Allele<TestLocus>>> _alleleRepo;
        private Mock<IGenotypeRepository<TestLocus, Guid>> _genotypeRepo;
        private Mock<ILogger<GenotypeCalculator<TestLocus, Allele<TestLocus>, Guid>>> _logger;
        private GenotypeCalculator<TestLocus, Allele<TestLocus>, Guid> _calculator;

        private FirstAllele _firstAllele = new FirstAllele();
        private SecondAllele _secondAllele = new SecondAllele();
        private ThirdAllele _thirdAllele = new ThirdAllele();

        public GenotypeCalculatorTests()
        {
            _alleleRepo = new Mock<IAlleleRepository<TestLocus, Allele<TestLocus>>>();
            _genotypeRepo = new Mock<IGenotypeRepository<TestLocus, Guid>>();
            _logger = new Mock<ILogger<GenotypeCalculator<TestLocus, Allele<TestLocus>, Guid>>>();
            _calculator = new GenotypeCalculator<TestLocus, Allele<TestLocus>, Guid>(
                _genotypeRepo.Object,
                _alleleRepo.Object, 
                new PunnetSquare<TestLocus, Guid>(),
                _logger.Object
            );

            _alleleRepo.Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Allele<TestLocus>>() 
                {
                    _firstAllele,
                    _secondAllele,
                    _thirdAllele
                });
        } // end method

        [Fact]
        public async Task Where_Did_Tan_Come_From()
        {
            var individual = new IndividualGenotype<TestLocus>()
            {
                Id = Guid.NewGuid(),
                Genotype = new Genotype<TestLocus>()
            };

            var otherParent = new IndividualGenotype<TestLocus>()
            {
                Id = Guid.NewGuid(),
                Genotype = new Genotype<TestLocus>()
                {
                    DominantAllele = _firstAllele
                }
            };

            var offspring = new List<IndividualGenotype<TestLocus>>()
            {
                new IndividualGenotype<TestLocus>()
                {
                    PaternalId = individual.Id,
                    MaternalId = otherParent.Id,
                    Genotype = new Genotype<TestLocus>()
                    {
                        DominantAllele = _firstAllele
                    }
                },
                new IndividualGenotype<TestLocus>()
                {
                    PaternalId = individual.Id,
                    MaternalId = otherParent.Id,
                    Genotype = new Genotype<TestLocus>()
                    {
                        DominantAllele = _secondAllele
                    }
                },
                new IndividualGenotype<TestLocus>()
                {
                    PaternalId = individual.Id,
                    MaternalId = otherParent.Id,
                    Genotype = new Genotype<TestLocus>()
                    {
                        DominantAllele = _thirdAllele
                    }
                }
            };

            _genotypeRepo.Setup(x => x.GetAsync(individual.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(individual);
            _genotypeRepo.Setup(x => x.GetAsync(otherParent.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(otherParent);
            _genotypeRepo.Setup(x => x.GetOffspringAsync(individual.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(offspring);

            var result = await _calculator.CalculateGenotypesAsync(individual.Id).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count());
        } // end method
    } // end class
} // end namespace