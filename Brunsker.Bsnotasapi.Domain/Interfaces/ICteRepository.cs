using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface ICteRepository
    {
        Task<IEnumerable<Cte>> BuscarCteAsync(ParametrosPesquisaCte pesquisa);
        Task<TotalizadoresCte> BuscarTotalizadoresCteAsync(DateTime? dataInicio, DateTime? dataFim, int seqCliente);
        Task<IEnumerable<EmpresasCliente>> BuscarEmpresasAsync(long seqCliente);
        Task<IEnumerable<TotalizadorNotasPorDia>> BuscarTotalizadoresGraficoAsync(FiltroTotalizadores filtro);
        
    }
}