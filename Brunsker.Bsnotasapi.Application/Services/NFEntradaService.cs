using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Interfaces;
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

namespace Brunsker.Bsnotasapi.Application.Services
{
    public class NFService : INFService
    {
        private readonly ILogger<INFService> _logger;

        public NFService(ILogger<NFService> logger)
        {
            _logger = logger;
        }

        public async Task<Stream> GerarPdfAsync(string xml)
        {
            try
            {
                _logger.LogInformation("Iniciou o processo de geracao de pdf");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();

                    var content = new StringContent("&xml_conteudo=" + HttpUtility.UrlEncode(xml), Encoding.UTF8, "application/x-www-form-urlencoded");

                    var httpResponseMessage = await httpClient.PostAsync("http://portal.brunsker.com.br:1234/testaDanfe.php", content);

                    httpResponseMessage.EnsureSuccessStatusCode();

                    return await httpResponseMessage.Content.ReadAsStreamAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return null;
            }
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

        public byte[] ExportaXmls(IEnumerable<NF> notas)
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
        public async Task<byte[]> ExportaPdfs(IEnumerable<NF> notas)
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
        public async Task<Stream> GerarCCeAsync(string xml)
        {
            try
            {
                _logger.LogInformation("Iniciou o processo de geracao de CCe em pdf");

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
    }
}
