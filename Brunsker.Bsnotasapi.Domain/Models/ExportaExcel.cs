using System.Collections.Generic;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class ExportaExcel<T> where T : class
    {
        public IEnumerable<T> Entities { get; set; }
    }
}