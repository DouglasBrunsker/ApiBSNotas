using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.Domain.Services
{
    public interface INfeEntradaService
    {
        Task<Totalizadores> BuscarTotalizadoresAsync(DateTime dataInicio, DateTime dataFim, int seqCliente);
        Task<IEnumerable<TotalizadorNotasPorDia>> BuscarTotalizadoresGraficoAsync(FiltroTotalizadores filtro);
        Task<IEnumerable<EmpresasCliente>> BuscarEmpresasAsync(long seqCliente);
        Task<IEnumerable<NotaFiscalEntrada>> BuscarNotasFiscaisEntrada(ParametrosPesquisaNfEntrada param);
        Task<IEnumerable<CFOP>> BuscarCfopNotaFiscalEntrada(long seqCliente);
        Task<IEnumerable<DetalheNotaFiscalEntrada>> BuscarDetalhesNotaFiscalEntradaAsync(long seqCliente, string chaveNfe);
        Task<IEnumerable<Fornecedores>> BuscarFornecedoresAsync(long seqCliente);
        Task<Stream> GerarPdfAsync(string xml);
        Task<Stream> GerarCCeAsync(string xml);
        MemoryStream ExportaExcel(IEnumerable<NFeToExport> notas);
        byte[] ExportaXmls(IEnumerable<NotaFiscalEntrada> notas);
        Task<byte[]> ExportaPdfs(IEnumerable<NotaFiscalEntrada> notas);
        Task<IEnumerable<ResultadoValidacaoPreEntrada>> ValidarPreEntrada(ValidarPreEntrada validar);
        Task ProcessaPreEntrada(ItensPedidoPre item);
        Task<IEnumerable<PedidoAssociado>> SelectPedidosAssociados(string chave, int seqCliente);
        Task<ParametrosCliente> SelectParametros(int seqCliente);
        Task<IEnumerable<ItemPedido>> SelectItensPedido(PesquisaItensPedido pesq);
    }
}
