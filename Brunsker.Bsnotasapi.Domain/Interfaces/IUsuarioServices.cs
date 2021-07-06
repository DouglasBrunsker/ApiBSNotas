using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface IUsuarioServices
    {
        string GeraToken(Usuario usuario);
    }
}