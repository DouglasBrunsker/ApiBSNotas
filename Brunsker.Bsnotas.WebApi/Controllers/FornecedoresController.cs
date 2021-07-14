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
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedorRepository _rep;
        private ILogger<FornecedoresController> _logger;

        public FornecedoresController(IFornecedorRepository rep, ILogger<FornecedoresController> logger)
        {
            _rep = rep;
            _logger = logger;
        }

        [HttpPost("BuscarFornecedores")]
        public async Task<IActionResult> BuscarClientes(FiltroPesquisaFornecedor filtro)
        {
            var fornecedores = await _rep.SelectFornecedores(filtro);

            return Ok(new Pagination<object>(filtro.Index, filtro.Length, fornecedores));
        }
    }
}