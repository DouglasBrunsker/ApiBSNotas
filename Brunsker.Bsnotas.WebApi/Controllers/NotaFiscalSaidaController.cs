using System.Threading.Tasks;
using Brunsker.Bsnotas.WebApi.Helpers;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brunsker.Bsnotas.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalSaidaController : ControllerBase
    {
        private readonly INFSaidaRepository _rep;
        private readonly ILogger<NotaFiscalSaidaController> _logger;

        public NotaFiscalSaidaController(INFSaidaRepository rep, ILogger<NotaFiscalSaidaController> logger)
        {
            _rep = rep;
            _logger = logger;
        }

        [HttpPost("BuscarNotas")]
        public async Task<IActionResult> BuscarNotas(FiltroBuscaNotasSaida filtro)
        {
            _logger.LogInformation("Buscando notas de saída.");

            var notas = await _rep.BuscaNotas(filtro);

            return Ok(new Pagination<NotaFiscalSaida>(filtro.Index, filtro.Length, notas));
        }

        [HttpPost("BuscarTotalizador")]
        public async Task<IActionResult> BuscarTotalizador(FiltroTotalizadores filtro)
        {
            _logger.LogInformation("Buscando totalizador.");

            var totalizador = await _rep.BuscarTotalizador(filtro);

            return Ok(totalizador);
        }

        [HttpPost("BuscarTotalizadorNotasDia")]
        public async Task<IActionResult> BuscarTotalizadorNotasDia(FiltroTotalizadores filtro)
        {
            _logger.LogInformation("Buscando totalizador de notas por dia.");

            var totalizador = await _rep.TotalizadorNotasEmitidasDia(filtro);

            return Ok(totalizador);
        }

        [HttpGet("BuscarEmpresas/{seqCliente}")]
        public async Task<IActionResult> BuscarEmpresas(long seqCliente)
        {
            _logger.LogInformation("Buscando empresas cadastradas.");

            var totalizador = await _rep.BuscarEmpresas(seqCliente);

            return Ok(totalizador);
        }

        [HttpGet("BuscarCFOPs/{seqCliente}")]
        public async Task<IActionResult> BuscarCFOPs(long seqCliente)
        {
            _logger.LogInformation("Buscando cfops.");

            var cfops = await _rep.BuscarCFOPs(seqCliente);

            return Ok(cfops);
        }
    }
}