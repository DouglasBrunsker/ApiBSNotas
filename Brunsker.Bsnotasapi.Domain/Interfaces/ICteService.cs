using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface ICteService
    {
        Task<Stream> GerarPdfAsync(string xml);
        Task<byte[]> ExportaPdfs(IEnumerable<Cte> ctes);
        byte[] ExportaXmls(IEnumerable<Cte> ctes);
        MemoryStream ExportaExcel(IEnumerable<CteToExport> ctes);
    }
}