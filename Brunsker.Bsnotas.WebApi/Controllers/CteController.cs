using System.Threading.Tasks;
using AutoMapper;
using Brunsker.Bsnotas.WebApi.Helpers;
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

        [HttpPost("BuscarCteEntrada/{index}/{length}")]
        public async Task<IActionResult> BuscarCteEntradaAsync(ParametrosPesquisaCteEntrada pesquisa, int index, int length)
        {
            var ctes = await _rep.BuscarCteEntradaAsync(pesquisa);

            var result = new Pagination<Cte>(index, length, ctes);

            if (ctes != null)
            {
                return Ok(result);
            }

            return NoContent();
        }
    }
}