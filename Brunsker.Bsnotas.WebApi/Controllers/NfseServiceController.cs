using Brunsker.Bsnotas.Application.Interfaces;
using Brunsker.Bsnotas.Application.Requests.SearchCompany;
using Brunsker.Bsnotas.Application.Requests.SearchNf;
using Brunsker.Bsnotas.Application.Requests.SearchNfse;
using Brunsker.Bsnotas.Application.Responses.Company;
using Brunsker.Bsnotas.Application.Responses.Nfse;
using Brunsker.Bsnotas.Application.Responses.NotasDia;
using Brunsker.Bsnotas.Application.Responses.Totalizador;
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
        private readonly INfseServicoService _nfseServicoService;

        public NfseServiceController(INfseServicoService nfseServicoService)
        {
            _nfseServicoService = nfseServicoService;
        }

        [HttpGet("get_totalizadores")]
        [QueryCommandsResponseTypes]
        public async Task<IEnumerable<TotalizadorResponse>> GetTotalizadoresAsync([FromQuery] SearchNfseRequest searchNfseRequest) =>
            await _nfseServicoService.GetTotalizadoresAsync(searchNfseRequest);

        [HttpGet("get_recebidas")]
        [QueryCommandsResponseTypes]
        public async Task<IEnumerable<NotasDiaResponse>> GetRecebidasDiaAsync([FromQuery] SearchNfseRequest searchNfseRequest) =>
            await _nfseServicoService.GetRecebidasDiaAsync(searchNfseRequest);

        [HttpGet("get_nfse")]
        [QueryCommandsResponseTypes]
        public async Task<IEnumerable<NfseResponse>> GetNfseAsync([FromQuery] SearchNfRequest searchNfRequest) =>
            await _nfseServicoService.GetNfseAsync(searchNfRequest);

        [HttpGet("get_companys")]
        [QueryCommandsResponseTypes]
        public async Task<IEnumerable<CompanyResponse>> GetEmpresasAsync([FromQuery] SearchCompanyRequest searchCompanyRequest) =>
            await _nfseServicoService.GetEmpresasAsync(searchCompanyRequest);
    }
}
