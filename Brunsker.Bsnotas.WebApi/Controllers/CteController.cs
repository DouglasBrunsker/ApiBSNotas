using System.Collections.Generic;
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

        public CteController(ICteRepository rep, IWebHostEnvironment env, ILogger<CteController> logger, IMapper mapper)
        {
            _rep = rep;
            _env = env;
            _logger = logger;
            _mapper = mapper;
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
    }
}