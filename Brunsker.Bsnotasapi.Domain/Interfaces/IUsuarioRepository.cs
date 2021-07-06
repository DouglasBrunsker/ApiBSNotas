using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<ParametrosCliente> SelectParametros(long id);
        Task InsertUsuario(Usuario usuario);
        Task<Usuario> SelectUsuarioPorEmail(string email);
        Task<Usuario> Login(string login, string senha);
    }
}