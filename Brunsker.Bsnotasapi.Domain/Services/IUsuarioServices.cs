using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Services
{
    public interface IUsuarioServices
    {
        Task CriaUsuario(Usuario usuario);
        string GeraToken(Usuario usuario);
        Task<Usuario> SelectUsuarioPorEmail(string email);
        Task<Usuario> Login(string login, string senha);
    }
}