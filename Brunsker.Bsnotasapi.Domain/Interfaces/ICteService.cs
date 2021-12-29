using System.IO;
using System.Threading.Tasks;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface ICteService
    {
        Task<Stream> GerarPdfAsync(string xml);
    }
}