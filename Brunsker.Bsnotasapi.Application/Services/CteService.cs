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
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotasapi.Application.Services
{
    public class CteService : ICteService
    {
        private readonly ILogger<ICteService> _logger;
        public CteService(ILogger<ICteService> logger)
        {
            _logger = logger;
        }
        public async Task<Stream> GerarPdfAsync(string xml)
        {
            try
            {
                _logger.LogInformation("Iniciou o processo de geracao de pdf");

                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Accept.Clear();

                    var content = new StringContent("&xml_conteudo=" + HttpUtility.UrlEncode(xml), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await http.PostAsync("http://portal.brunsker.com.br:1234/testaDaCTe.php", content);

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
        public async Task<byte[]> ExportaPdfs(IEnumerable<Cte> ctes)
        {
            byte[] bytes = null;

            try
            {
                string file_temp_name = @"C:\CTE\DaCTes" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + ".zip";

                if (ctes.Any())
                {
                    using (FileStream zipToOpen = new FileStream(file_temp_name, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                        {
                            foreach (var cte in ctes)
                            {
                                XmlDocument xml = new XmlDocument();

                                string xmlConteudo = cte.ARQUIVO_XML;

                                xml.LoadXml(cte.ARQUIVO_XML);

                                using (var http = new HttpClient())
                                {
                                    http.DefaultRequestHeaders.Accept.Clear();

                                    var content = new StringContent("&xml_conteudo=" + HttpUtility.UrlEncode(xmlConteudo), Encoding.UTF8, "application/x-www-form-urlencoded");

                                    HttpResponseMessage response = await http.PostAsync("http://portal.brunsker.com.br:1234/testaDaCTe.php", content);

                                    response.EnsureSuccessStatusCode();

                                    Stream danfe = response.Content.ReadAsStreamAsync().Result;

                                    ZipArchiveEntry readmeEntry = archive.CreateEntry(cte.CHAVE + ".pdf");

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
        public byte[] ExportaXmls(IEnumerable<Cte> ctes)
        {
            byte[] bytes = null;

            try
            {
                string file_temp_name = @"C:\CTe\xml_ctes" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + ".zip";

                if (ctes.Any())
                {
                    using (FileStream zipToOpen = new FileStream(file_temp_name, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                        {
                            foreach (var cte in ctes)
                            {
                                XmlDocument xml = new XmlDocument();

                                xml.LoadXml(cte.ARQUIVO_XML);

                                byte[] byteArray = Encoding.ASCII.GetBytes(cte.ARQUIVO_XML);

                                Stream xml_stream = new MemoryStream(byteArray);

                                ZipArchiveEntry readmeEntry = archive.CreateEntry(cte.CHAVE + ".xml");

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
    }
}