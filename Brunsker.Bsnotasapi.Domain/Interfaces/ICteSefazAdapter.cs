using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface ICteSefazAdapter
    {
        Task ManifestaCte(Manifestacao manifestacao, string webRootPath);
    }
}