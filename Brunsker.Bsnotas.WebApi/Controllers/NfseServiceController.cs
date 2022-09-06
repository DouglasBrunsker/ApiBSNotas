using Brunsker.Bsnotas.Domain.Interfaces;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotas.WebApi.ControllersAttributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NfseServiceController : ControllerBase
    {
        private readonly INfseServiceRepository _nfseServiceRepository;

        public NfseServiceController(INfseServiceRepository nfseServiceRepository)
        {
            _nfseServiceRepository = nfseServiceRepository;
        }

        [QueryCommandsResponseTypes]
        [HttpGet("get_totalizadores")]
        public async Task<IEnumerable<SearchNfse>> GetTotalizadoresAsync([FromBody] SearchNfse searchNfse) =>
            await _nfseServiceRepository.GetTotalizadoresAsync(searchNfse);

        [QueryCommandsResponseTypes]
        [HttpGet("get_recebidas")]
        public async Task<IEnumerable<SearchNfse>> GetRecebidasDiaAsync([FromBody] SearchNfse searchNfse) =>
            await _nfseServiceRepository.GetRecebidasDiaAsync(searchNfse);

        [QueryCommandsResponseTypes]
        [HttpGet("get_nfse")]
        public async Task<IEnumerable<SearchNf>> GetNfseAsync([FromBody] SearchNf searchNf) =>
            await _nfseServiceRepository.GetNfseAsync(searchNf);
    }
}
