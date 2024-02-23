using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.Heredity.Interfaces
{
    public interface IGenotypeCalculator<TLocus, TId>
        where TLocus : Locus, new()
    {
        Task<IEnumerable<Genotype<TLocus>>> CalculateGenotypesAsync(TId individualId, CancellationToken token = default);
    } // end interface
} // end namespace