using System.Collections.Generic;
using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface ICteRepository
    {
        Task<IEnumerable<Cte>> BuscarCteEntradaAsync(ParametrosPesquisaCteEntrada pesquisa);
    }
}