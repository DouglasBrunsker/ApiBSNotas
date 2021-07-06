using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface ISefazApiAdapter
    {
        Task ManifestaNotas(Manifestacao manifestacao, string webRootPath);
    }
}
