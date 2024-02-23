using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity.Interfaces
{
    public interface IGenotypeRepository<TLocus, TId>
        where TLocus : Locus, new()
    {
        Task<IndividualGenotype<TLocus, TId>> GetAsync(TId individualId, CancellationToken token = default);
        Task<IEnumerable<IndividualGenotype<TLocus, TId>>?> GetOffspringAsync(TId parentId, CancellationToken token = default);
    } // end interface
} // end namespace