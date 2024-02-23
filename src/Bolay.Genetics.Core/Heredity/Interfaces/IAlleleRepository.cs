using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity.Interfaces
{
    public interface IAlleleRepository<TLocus, TAllele>
        where TLocus : Locus, new()
        where TAllele : Allele<TLocus>
    {
        Task<IEnumerable<TAllele>> GetAsync(CancellationToken token = default);
    } // end interface
} // end namespace