using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface INfeEntradaService
    {
        Task<Stream> GerarPdfAsync(string xml);
        Task<Stream> GerarCCeAsync(string xml);
        MemoryStream ExportaExcel(IEnumerable<NFeToExport> notas);
        byte[] ExportaXmls(IEnumerable<NotaFiscalEntrada> notas);
        Task<byte[]> ExportaPdfs(IEnumerable<NotaFiscalEntrada> notas);
    }
}
