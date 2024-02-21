using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolay.Genetics.Core.Models;

namespace Bolay.Genetics.Core.PunnetSquares
{
    public class ParentGenotypeRatio<TLocus>
        where TLocus : Locus, new()
    {
        public bool IsPaternal { get; set; }
        public IEnumerable<GenotypeRatio<TLocus>> GenotypeRatios { get; set; }
    } // end class
} // end namespace