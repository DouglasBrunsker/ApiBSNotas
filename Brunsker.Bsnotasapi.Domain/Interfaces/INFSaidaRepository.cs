using System.Collections.Generic;
using System.Threading.Tasks;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface INFSaidaRepository
    {
        Task<IEnumerable<NotaFiscalSaida>> BuscaNotas(FiltroBuscaNotasSaida filtro);
        Task<IEnumerable<EmpresasCliente>> BuscarEmpresas(long seqCliente);
        Task<IEnumerable<TotalizadorNotasPorDia>> TotalizadorNotasEmitidasDia(FiltroTotalizadores filtro);
        Task<TotalizadorSaida> BuscarTotalizador(FiltroTotalizadores filtro);
        Task<IEnumerable<CFOP>> BuscarCFOPs(long seqCliente);
    }
}