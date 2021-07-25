using System.Threading.Tasks;
using Brunsker.Bsnotas.WebApi.Helpers;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brunsker.Bsnotas.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _rep;
        private ILogger<ClientesController> _logger;

        public ClientesController(IClienteRepository rep, ILogger<ClientesController> logger)
        {
            _rep = rep;
            _logger = logger;
        }

        [HttpPost("BuscarClientes/{index}/{length}")]
        public async Task<IActionResult> BuscarClientes(FiltroPesquisaClientes filtro, int index, int length)
        {
            var clientes = await _rep.SelectClientes(filtro);

            return Ok(new Pagination<Cliente>(index, length, clientes));
        }
    }
}