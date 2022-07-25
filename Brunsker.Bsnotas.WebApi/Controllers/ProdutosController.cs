using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brunsker.Bsnotas.Domain.Models;
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
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _rep;
        private ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoRepository rep, ILogger<ProdutosController> logger)
        {
            _rep = rep;
            _logger = logger;
        }

        [HttpPost("BuscarProdutos")]
        public async Task<IActionResult> BuscarProdutos(FiltroPesquisaProdutos filtro)
        {
            var produtos = await _rep.SelectProdutos(filtro);

            if (produtos != null)
            {
                return Ok(new Pagination<Produto>(filtro.Index, filtro.Length, produtos));
            }

            return NoContent();
        }

        /*[HttpPost("BuscarProdutos")]
        public async Task<IActionResult> BuscarProdutos(FiltroPesquisaProdutos filtro)
        {
            var produtos = await _rep.SelectProdutos(filtro);

            if (produtos != null)
            {
                return Ok(new Pagination<Produto>(filtro.Index, filtro.Length, produtos));
            }

            return NoContent();
        }*/

        //[HttpGet("buscarCprod/chave")]
        //public async Task<IEnumerable<CodProd>> BuscaCPROD(string chave)
        //{
        //    var CPROD = await _rep.SearchCPROD(chave);

        //    return CPROD;
        //}

        //[HttpGet("ExibirICMS/")]
        //public async Task<ICMS> ExibirICMS(string chave, string cprod)

        //{
        //    var ICMS = await _rep.ExibirICMS(chave, cprod);

        //    return ICMS;
        //}
    }
}