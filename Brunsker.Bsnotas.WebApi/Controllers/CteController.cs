using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brunsker.Bsnotas.WebApi.Helpers;
using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brunsker.Bsnotas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CteController : ControllerBase
    {
        private readonly ICteRepository _rep;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CteController> _logger;
        private readonly IMapper _mapper;
        private readonly ICteService _services;
        private readonly ICteSefazAdapter _sefazServices;

        public CteController(ICteRepository rep, IWebHostEnvironment env, ILogger<CteController> logger, IMapper mapper, ICteService services, ICteSefazAdapter sefazServices)
        {
            _rep = rep;
            _env = env;
            _logger = logger;
            _mapper = mapper;
            _services = services;
            _sefazServices = sefazServices;
        }

        [HttpGet("BuscarEmpresas/{seqCliente}")]
        public async Task<IActionResult> BuscarEmpresasAsync(long seqCliente)
        {

            var listaEmpresas = await _rep.BuscarEmpresasAsync(seqCliente);

            if (listaEmpresas != null)
            {
                return Ok(listaEmpresas);
            }

            return NoContent();
        }

        [HttpPost("BuscarTotalizadoresGrafico")]
        public async Task<IActionResult> BuscarTotalizadoresGrafico(FiltroTotalizadores filtro)
        {
            var result = await _rep.BuscarTotalizadoresGraficoAsync(filtro);

            return Ok(result);
        }

        [HttpPost("BuscarTotalizadores")]
        public async Task<IActionResult> BuscarTotalizadoresAsync(FiltroTotalizadores filtro)
        {
            TotalizadoresCte totalizadores = await _rep.BuscarTotalizadoresCteAsync(filtro.DataInicial, filtro.DataFinal, filtro.SeqCliente);

            if (totalizadores != null)
            {
                return Ok(totalizadores);
            }

            return NoContent();
        }

        [HttpPost("BuscarCte/{index}/{length}")]
        public async Task<IActionResult> BuscarCteAsync(ParametrosPesquisaCte pesquisa, int index, int length)
        {
            var ctes = await _rep.BuscarCteAsync(pesquisa);

            var result = new Pagination<Cte>(index, length, ctes);

            if (ctes != null)
            {
                return Ok(result);
            }

            return NoContent();
        }

        [HttpPost("BuscarNFeVinculadasCTe/")]
        public async Task<IActionResult> BuscarNFeVinculadasCTeAsync(NfeVinculadasCTe pesquisa)
        {
            var chaves = await _rep.BuscarNFeVinculadasCTeAsync(pesquisa);

            if (chaves != null)
            {
                return Ok(chaves);
            }

            return NoContent();
        }

        [HttpGet("GerarPdf/{chave}")]
        public async Task<IActionResult> GerarPdfAsync(string chave)
        {
            string xml = await _rep.SelectArquivoXml(chave);

            var pdf = await _services.GerarPdfAsync(xml);

            if (pdf != null)
            {
                var file = File(pdf, "application/pdf");

                return file;
            }

            return NoContent();
        }

        [HttpPost("ExportaPdfs")]
        public async Task<IActionResult> ExportaPdf(IEnumerable<Cte> filtro)
        {
            var bytes = await _services.ExportaPdfs(filtro);

            if (bytes.Length > 0)
            {
                return File(bytes, "application/zip", "DaCTes.zip");
            }
            return NoContent();
        }

        [HttpPost("ExportaXml")]
        public IActionResult ExportaXml(IEnumerable<Cte> filtro)
        {
            var bytes = _services.ExportaXmls(filtro);

            if (bytes.Length > 0)
            {
                return File(bytes, "application/zip");
            }
            return NoContent();
        }

        [HttpPost("ExportaExcel")]
        public async Task<IActionResult> ExportaExcelAsync(ParametrosPesquisaCte filtro)
        {
            var ctes = await _rep.BuscarCteAsync(filtro);

            var ctesDto = _mapper.Map<IEnumerable<CteToExport>>(ctes);

            MemoryStream excelMemoryStream = _services.ExportaExcel(ctesDto);

            if (excelMemoryStream == null)
            {
                return NoContent();
            }
            return File(excelMemoryStream, "application/vnd.ms-excel");
        }

        [HttpPost("Manifestacao")]
        public async Task<IActionResult> Manifestacao(IEnumerable<Manifestacao> manifestacoes)
        {
            try
            {
                if (manifestacoes.Any())
                {
                    foreach (var manifestacao in manifestacoes)
                    {
                        await _sefazServices.ManifestaCte(manifestacao, Path.Combine(_env.WebRootPath, "certificados/"));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }

            return Ok();
        }
    }
}