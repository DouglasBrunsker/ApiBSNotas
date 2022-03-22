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
        Task<RecepcaoEventoCte> SelectRelacaoWebServices(int seq_cliente, string cnpj, int tpAmb, int codUf);
        Task ConfirmaManifestacaoCte(string cStat, string dhRegEvento, string nProt, string chCTe, int seqCliente, string log, string tpEvento);
        Task LogErroManifestacaoCte(string cStat, string xMotivo, string chCte, int seqCliente);
    }
}