using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Brunsker.Bsnotas.WebApi.Helpers;
using Brunsker.Bsnotasapi.Domain.Dtos;
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
        private readonly INFService _sevices;
        private readonly ILogger<NotaFiscalSaidaController> _logger;
        private readonly IMapper _mapper;

        public NotaFiscalSaidaController(INFSaidaRepository rep, INFService sevices, ILogger<NotaFiscalSaidaController> logger, IMapper mapper)
        {
            _rep = rep;
            _sevices = sevices;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("BuscarNotas")]
        public async Task<IActionResult> BuscarNotas(FiltroBuscaNotasSaida filtro)
        {
            _logger.LogInformation("Buscando notas de saída.");

            var notas = await _rep.BuscaNotas(filtro);

            return Ok(new Pagination<NF>(filtro.Index, filtro.Length, notas));
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

        [HttpGet("GerarPdf/{chave}")]
        public async Task<IActionResult> GerarPdfAsync(string chave)
        {
            string xml = await _rep.SelectArquivoXml(chave);

            var pdf = await _sevices.GerarPdfAsync(xml);

            if (pdf != null)
            {
                var file = File(pdf, "application/pdf");

                return file;
            }

            return NoContent();
        }
        [HttpGet("GerarCCePDF/{chave}")]
        public async Task<IActionResult> GerarCCePDF(string chave)
        {
            string xml = await _rep.SelectArquivoXmlCCe(chave);

            var pdf = await _sevices.GerarCCeAsync(xml);

            if (pdf != null)
            {
                var file = File(pdf, "application/pdf");

                return file;
            }

            return NoContent();
        }

        [HttpPost("ExportaExcel")]
        public async Task<IActionResult> ExportaExcelAsync(FiltroBuscaNotasSaida filtro)
        {
            var notas = await _rep.BuscaNotas(filtro);

            var notasDto = _mapper.Map<IEnumerable<NFeToExport>>(notas);

            MemoryStream excelMemoryStream = _sevices.ExportaExcel(notasDto);

            if (excelMemoryStream == null)
            {
                return NoContent();
            }
            return File(excelMemoryStream, "application/vnd.ms-excel");
        }

        [HttpPost("ExportaXml")]
        public IActionResult ExportaXml(IEnumerable<NF> filtro)
        {
            var bytes = _sevices.ExportaXmls(filtro);

            if (bytes.Length > 0)
            {
                return File(bytes, "application/zip");
            }
            return NoContent();
        }

        [HttpPost("ExportaPdfs")]
        public async Task<IActionResult> ExportaPdf(IEnumerable<NF> filtro)
        {
            var bytes = await _sevices.ExportaPdfs(filtro);

            if (bytes.Length > 0)
            {
                return File(bytes, "application/zip", "Danfes.zip");
            }
            return NoContent();
        }
    }
}