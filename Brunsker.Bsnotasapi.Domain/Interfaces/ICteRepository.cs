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
        Task<IEnumerable<TotalizadorCtePorDia>> BuscarTotalizadoresGraficoAsync(FiltroTotalizadores filtro);
        Task<IEnumerable<NfeVinculadasCTe>> BuscarNFeVinculadasCTeAsync(NfeVinculadasCTe pesquisa);
        Task<string> SelectArquivoXml(string chave);
    }
}