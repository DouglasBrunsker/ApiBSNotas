using Brunsker.Bsnotas.Application.Interfaces;
using Brunsker.Bsnotas.Application.Requests.SearchCompany;
using Brunsker.Bsnotas.Application.Requests.SearchNf;
using Brunsker.Bsnotas.Application.Requests.SearchNfse;
using Brunsker.Bsnotas.Application.Responses.Company;
using Brunsker.Bsnotas.Application.Responses.Nfse;
using Brunsker.Bsnotas.Application.Responses.NotasDia;
using Brunsker.Bsnotas.Application.Responses.Totalizador;
using Brunsker.Bsnotas.WebApi.ControllersAttributes;
using Brunsker.Bsnotas.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [QueryCommandsResponseTypes]
    public class NfseServiceController : ControllerBase
    {
        private readonly INfseServicoService _nfseServicoService;

        public NfseServiceController(INfseServicoService nfseServicoService)
        {
            _nfseServicoService = nfseServicoService;
        }

        [HttpGet("get_totalizadores")]
        public async Task<IEnumerable<TotalizadorResponse>> GetTotalizadoresAsync([FromQuery] SearchNfseRequest searchNfseRequest) =>
            await _nfseServicoService.GetTotalizadoresAsync(searchNfseRequest);

        [HttpGet("get_recebidas")]
        public async Task<IEnumerable<NotasDiaResponse>> GetRecebidasDiaAsync([FromQuery] SearchNfseRequest searchNfseRequest) =>
            await _nfseServicoService.GetRecebidasDiaAsync(searchNfseRequest);

        [HttpGet("get_nfse")]
        public async Task<Pagination<NfseResponse>> GetNfseAsync([FromQuery] int index, [FromQuery] int length, [FromQuery] SearchNfRequest searchNfRequest) =>
            new Pagination<NfseResponse>(index, length, await _nfseServicoService.GetNfseAsync(searchNfRequest));

        [HttpGet("get_companys")]
        public async Task<IEnumerable<CompanyResponse>> GetEmpresasAsync([FromQuery] SearchCompanyRequest searchCompanyRequest) =>
            await _nfseServicoService.GetEmpresasAsync(searchCompanyRequest);

        [HttpGet("generate_pdf")]
        public async Task<Stream> GetPdfBySeqArquivoXmlNfseAsync(int seqArquivoXmlNfse) =>
            await _nfseServicoService.GetPdfBySeqArquivoXmlNfseAsync(seqArquivoXmlNfse);
    }
}
