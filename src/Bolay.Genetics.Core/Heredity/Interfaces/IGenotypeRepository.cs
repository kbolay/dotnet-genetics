using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity.Interfaces
{
    public interface IGenotypeRepository<TAllele, TLocus, TId>
        where TAllele : Allele
        where TLocus : Locus<TAllele>, new()
    {
        Task<IndividualGenotype<TAllele, TLocus, TId>> GetAsync(TId individualId, CancellationToken token = default);
        Task<IEnumerable<IndividualGenotype<TAllele, TLocus, TId>>?> GetOffspringAsync(TId parentId, CancellationToken token = default);
    } // end interface
} // end namespace