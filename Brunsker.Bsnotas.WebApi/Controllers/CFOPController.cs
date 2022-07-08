using Brunsker.Bsnotas.Domain.Interfaces;
using Brunsker.Bsnotas.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace Brunsker.Bsnotas.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CFOPController
    {
        private readonly ICadastroCFOPRepository _cfop;

        public CFOPController(ICadastroCFOPRepository cfop)
        {
            _cfop = cfop;
        }

        [HttpPost("CadastrarCFOP{seqCliente}/{stringBanco}/{cfopEnd}/{cfoSaida}")]
        public async Task CadastrarCFOP(int seqCliente, string stringBanco, int cfopEnd, int cfoSaida, string descricao)
        {
            await _cfop.CadastrarCFOP(seqCliente, stringBanco, cfopEnd, cfoSaida, descricao);

        }
    }
}
