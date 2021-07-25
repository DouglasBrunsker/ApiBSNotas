using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface INFService
    {
        Task<Stream> GerarPdfAsync(string xml);
        Task<Stream> GerarCCeAsync(string xml);
        MemoryStream ExportaExcel(IEnumerable<NFeToExport> notas);
        byte[] ExportaXmls(IEnumerable<NF> notas);
        Task<byte[]> ExportaPdfs(IEnumerable<NF> notas);
    }
}
