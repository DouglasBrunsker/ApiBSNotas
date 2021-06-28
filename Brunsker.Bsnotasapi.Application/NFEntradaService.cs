using Brunsker.Bsnotas.Domain.Adapters;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotas.Domain.Services;
using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Models;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Brunsker.Bsnotas.Application
{
    public class NfeEntradaService : INfeEntradaService
    {
        private readonly ILogger<NfeEntradaService> _logger;
        private IOracleRepositoryAdapter _rep;

        public NfeEntradaService(ILogger<NfeEntradaService> logger, IOracleRepositoryAdapter oracleRepository)
        {
            _logger = logger;
            _rep = oracleRepository;
        }

        public Task<IEnumerable<EmpresasCliente>> BuscarEmpresasAsync(long seqCliente)
        {

            _logger.LogInformation("Iniciou o processo de busca de empresas  {CodigoCliente}", seqCliente);

            var resultado = _rep.BuscarEmpresasAsync(seqCliente);

            _logger.LogInformation("busca de empresas concluida com sucesso.");

            return resultado;
        }
        public Task<IEnumerable<NotaFiscalEntrada>> BuscarNotasFiscaisEntrada(ParametrosPesquisaNfEntrada param)
        {
            _logger.LogInformation("Realizando busca de ntoas fiscais no BD com os seguintes criterios de pesquisa: {@CriteriosPesquisa}", new { Criterios = param });

            var resultado = _rep.BuscarNotasFiscaisEntradaAsync(param);

            _logger.LogInformation("pesquisa de notas fiscais de entrada concluida com sucesso.");

            return resultado;
        }
        public Task<Totalizadores> BuscarTotalizadoresAsync(DateTime dataInicio, DateTime dataFim, int seqCliente)
        {

            _logger.LogInformation("Realizando busca de totalizadores no BD com os seguintes criterios de pesquisa: {@CriteriosPesquisa}",
               new
               {
                   seqCliente = seqCliente,
                   dataInicio = dataInicio,
                   dataFim = dataFim
               });

            var resultado = _rep.BuscarTotalizadoresAsync(dataInicio, dataFim, seqCliente);

            _logger.LogInformation("pesquisa de totalizadores concluida com sucesso.");

            return resultado;
        }
        public Task<IEnumerable<CFOP>> BuscarCfopNotaFiscalEntrada(long seqCliente)
        {
            _logger.LogInformation("Realizando busca de cfop no BD com os seguintes criterios de pesquisa: {@CriteriosPesquisa}", new { Criterios = seqCliente });

            var resultado = _rep.BuscarCfopNotaFiscalEntradaAsync(seqCliente);

            _logger.LogInformation("pesquisa de totalizadores concluida com sucesso.");

            return resultado;
        }
        public Task<IEnumerable<DetalheNotaFiscalEntrada>> BuscarDetalhesNotaFiscalEntradaAsync(long seqCliente, string chaveNfe)
        {
            _logger.LogInformation("Realizando busca de detalhes da nota fiscal no BD com os seguintes criterios de pesquisa: {@CriteriosPesquisa}", new { Criterios = seqCliente });

            var resultado = _rep.BuscarDetalhesNotaFiscalEntradaAsync(seqCliente, chaveNfe);

            _logger.LogInformation("pesquisa de detalhes da nota fiscal concluida com sucesso.");

            return resultado;
        }
        public Task<IEnumerable<Fornecedores>> BuscarFornecedoresAsync(long seqCliente)
        {
            _logger.LogInformation("Realizando busca de fornecedores no BD com os seguintes criterios de pesquisa: {@CriteriosPesquisa}", new { Criterios = seqCliente });

            var resultado = _rep.BuscarFornecedoresAsync(seqCliente);

            _logger.LogInformation("pesquisa de fornecedores concluida com sucesso.");

            return resultado;
        }
        public async Task<Stream> GerarPdfAsync(string chave)
        {
            try
            {
                _logger.LogInformation("Iniciou o processo de geracao de pdf");

                var xml = await _rep.SelectArquivoXml(chave);

                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Accept.Clear();

                    var content = new StringContent("&xml_conteudo=" + HttpUtility.UrlEncode(xml), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await http.PostAsync("http://portal.brunsker.com.br:1234/testaDanfe.php", content);

                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStreamAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return null;
            }
        }
        public async Task<IEnumerable<TotalizadorNotasPorDia>> BuscarTotalizadoresGraficoAsync(FiltroTotalizadores filtro)
        {
            var resultado = await _rep.BuscarTotalizadoresGraficoAsync(filtro);

            return resultado;
        }
        public MemoryStream ExportaExcel(IEnumerable<NFeToExport> notas)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                if (notas.Any())
                {
                    using (ExcelPackage pack = new ExcelPackage(ms))
                    {
                        ExcelWorksheet ws = pack.Workbook.Worksheets.Add("NFe");

                        ws.Cells["A1"].LoadFromCollection(notas, true);

                        ws.Cells.AutoFitColumns();

                        ws.Row(1).Style.Font.Bold = true;

                        ws.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;

                        ws.Row(1).Style.Fill.BackgroundColor.SetColor(Color.MediumSeaGreen);

                        pack.Save();

                        ms.Position = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return (ms);
        }
        public byte[] ExportaXmls(IEnumerable<NotaFiscalEntrada> notas)
        {
            byte[] bytes = null;

            try
            {
                string file_temp_name = @"C:\NFE\xml_nfes" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + ".zip";

                if (notas.Any())
                {
                    using (FileStream zipToOpen = new FileStream(file_temp_name, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                        {
                            foreach (var nota in notas)
                            {
                                XmlDocument xml = new XmlDocument();

                                xml.LoadXml(nota.ARQUIVO_XML);

                                byte[] byteArray = Encoding.ASCII.GetBytes(nota.ARQUIVO_XML);

                                Stream xml_stream = new MemoryStream(byteArray);

                                ZipArchiveEntry readmeEntry = archive.CreateEntry(nota.CHAVE + ".xml");

                                using (Stream writer = readmeEntry.Open())
                                {
                                    xml_stream.CopyTo(writer);
                                }
                            }
                        }
                    }
                    bytes = System.IO.File.ReadAllBytes(file_temp_name);

                    System.IO.File.Delete(file_temp_name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return bytes;
        }
        public async Task<byte[]> ExportaPdfs(IEnumerable<NotaFiscalEntrada> notas)
        {
            byte[] bytes = null;

            try
            {
                string file_temp_name = @"C:\NFE\danfes" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + ".zip";

                if (notas.Any())
                {
                    using (FileStream zipToOpen = new FileStream(file_temp_name, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                        {
                            foreach (var nota in notas)
                            {
                                XmlDocument xml = new XmlDocument();

                                string xmlConteudo = nota.ARQUIVO_XML;

                                xml.LoadXml(nota.ARQUIVO_XML);

                                using (var http = new HttpClient())
                                {
                                    http.DefaultRequestHeaders.Accept.Clear();

                                    var content = new StringContent("&xml_conteudo=" + HttpUtility.UrlEncode(xmlConteudo), Encoding.UTF8, "application/x-www-form-urlencoded");

                                    HttpResponseMessage response = await http.PostAsync("http://portal.brunsker.com.br:1234/testaDanfe.php", content);

                                    response.EnsureSuccessStatusCode();

                                    Stream danfe = response.Content.ReadAsStreamAsync().Result;

                                    ZipArchiveEntry readmeEntry = archive.CreateEntry(nota.CHAVE + ".pdf");

                                    using (Stream writer = readmeEntry.Open())
                                    {
                                        danfe.CopyTo(writer);
                                    }
                                }
                            }
                        }
                    }

                    bytes = System.IO.File.ReadAllBytes(file_temp_name);

                    System.IO.File.Delete(file_temp_name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return bytes;
        }

        public async Task<Stream> GerarCCeAsync(string chave)
        {
            try
            {
                _logger.LogInformation("Iniciou o processo de geracao de CCe em pdf");

                var xml = await _rep.SelectArquivoXmlCCe(chave);

                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Accept.Clear();
                    var keyValues = new List<KeyValuePair<string, string>>();
                    keyValues.Add(new KeyValuePair<string, string>("xml_conteudo", xml));
                    FormUrlEncodedContent content = new FormUrlEncodedContent(keyValues);

                    HttpResponseMessage response = await http.PostAsync("http://portal.brunsker.com.br:1234/testaDacce.php", content);

                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStreamAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return null;
            }
        }

        public async Task<IEnumerable<ResultadoValidacaoPreEntrada>> ValidarPreEntrada(ValidarPreEntrada validar)
        {
            _logger.LogInformation("Iniciou o processo de validacao Pre Entrada.");

            var result = await _rep.ValidaPreEntrada(validar);

            _logger.LogInformation("Validacao recebida com sucesso.");

            return result;
        }
        public async Task ProcessaPreEntrada(string chave, int seqCliente, long? numped)
        {
            _logger.LogInformation("Iniciou o processo de  Pre Entrada.");

            await _rep.ProcessaPreEntrada(chave, seqCliente, numped);

            _logger.LogInformation("Pre Entrada realizada com sucesso.");
        }
        public async Task<IEnumerable<PedidoAssociado>> SelectPedidosAssociados(string chave, int seqCliente)
        {
            _logger.LogInformation("Iniciou o processo de  busca por pedidos associados.");

            return await _rep.SelectPedidosAssociados(chave, seqCliente);
        }

        public async Task<ParametrosCliente> SelectParametros(int seqCliente)
        {
            _logger.LogInformation("Buscando parametros...");

            return await _rep.SelectParametros(seqCliente);
        }
        public async Task<IEnumerable<ItemPedido>> SelectItensPedido(PesquisaItensPedido pesq)
        {
            _logger.LogInformation("Buscando itens do pedido...");

            return await _rep.SelectItensPedido(pesq);
        }
    }
}
