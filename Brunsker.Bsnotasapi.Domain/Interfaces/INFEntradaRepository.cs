using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Bsnotasapi.Domain.Interfaces
{
    public interface INFEntradaRepository
    {
        Task<string> SelectArquivoXml(string chave);
        Task<string> SelectArquivoXmlCCe(string chave);
        Task<IEnumerable<EmpresasCliente>> BuscarEmpresasAsync(long seqCliente);
        Task<Totalizadores> BuscarTotalizadoresAsync(DateTime dataInicio, DateTime dataFim, int seqCliente);
        Task<IEnumerable<TotalizadorNotasPorDia>> BuscarTotalizadoresGraficoAsync(FiltroTotalizadores filtro);
        Task<IEnumerable<NotaFiscalEntrada>> BuscarNotasFiscaisEntradaAsync(ParametrosPesquisaNfEntrada pesquisa);
        Task<IEnumerable<CFOP>> BuscarCfopNotaFiscalEntradaAsync(long seqCliente);
        Task<IEnumerable<DetalheNotaFiscalEntrada>> BuscarDetalhesNotaFiscalEntradaAsync(long seqCliente, string chaveNfe);
        Task<IEnumerable<Fornecedores>> BuscarFornecedoresAsync(long seqCliente);
        Task<RecepcaoEvento> SelectRelacaoWebServices(int seq_cliente, string cnpj);
        Task ConfirmaManifestacao(string chave, string codigoEvento);
        Task<IEnumerable<ResultadoValidacaoPreEntrada>> ValidaPreEntrada(ValidarPreEntrada validar);
        Task ProcessaPreEntrada(ItensPedidoPre item);
        Task<IEnumerable<PedidoAssociado>> SelectPedidosAssociados(string chave, int seqCliente);
        Task<IEnumerable<ItemPedido>> SelectItensPedido(PesquisaItensPedido pesq);
        Task<IEnumerable<object>> BuscarLivroFiscal(ParametroGeracaoLivroFiscal parametro);
        Task<IEnumerable<Contas>> SelectContas(long seqCliente);
    }
}
