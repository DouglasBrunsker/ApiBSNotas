using Brunsker.Bsnotas.Domain.Models;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.Domain.Interfaces
{
    public interface ICadastroCFOPRepository
    {
        Task<CadastroCFOP> CadastrarCFOP(int seqCliente, string stringBanco, int cfopEnd, int cfoSaida, string descricao);
    }
}
