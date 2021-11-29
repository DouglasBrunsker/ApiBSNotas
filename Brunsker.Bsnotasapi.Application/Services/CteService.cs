using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Microsoft.Extensions.Logging;

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
    }
}