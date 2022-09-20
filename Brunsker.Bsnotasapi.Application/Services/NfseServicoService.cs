using Brunsker.Bsnotas.Application.AutoMapperConfigurations;
using Brunsker.Bsnotas.Application.Interfaces;
using Brunsker.Bsnotas.Application.Requests.GeneratePdf;
using Brunsker.Bsnotas.Application.Requests.Searchs;
using Brunsker.Bsnotas.Application.Responses.Company;
using Brunsker.Bsnotas.Application.Responses.Notas;
using Brunsker.Bsnotas.Application.Responses.Pdf;
using Brunsker.Bsnotas.Application.Responses.Totalizador;
using Brunsker.Bsnotas.Domain.Interfaces;
using Brunsker.Bsnotas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.Application.Services
{
    public class NfseServicoService : INfseServicoService
    {
        private readonly INfseServiceRepository _nfseServiceRepository;

        public NfseServicoService(INfseServiceRepository nfseServiceRepository)
        {
            _nfseServiceRepository = nfseServiceRepository;
        }

        public async Task<IEnumerable<TotalizadorResponse>> GetTotalizadoresAsync(SearchNfseRequest searchNfseRequest)
        {
            var searchNfse = searchNfseRequest.MapTo<SearchNfseRequest, SearchNfse>();
            
            var totalizadoresEnumerable = await _nfseServiceRepository.GetTotalizadoresAsync(searchNfse);

            return totalizadoresEnumerable.MapTo<IEnumerable<Totalizador>, IEnumerable<TotalizadorResponse>>();
        }

        public async Task<IEnumerable<NotasDiaResponse>> GetRecebidasDiaAsync(SearchNfseRequest searchNfseRequest)
        {
            var searchNfse = searchNfseRequest.MapTo<SearchNfseRequest, SearchNfse>();

            var notasDiaEnumerable = await _nfseServiceRepository.GetRecebidasDiaAsync(searchNfse);

            return notasDiaEnumerable.MapTo<IEnumerable<NotasDia>, IEnumerable<NotasDiaResponse>>();
        }

        public async Task<IEnumerable<NfseResponse>> GetNfseAsync(SearchNfRequest searchNfRequest)
        {
            var searchNf = searchNfRequest.MapTo<SearchNfRequest, SearchNf>();
            
            var nfseEnumerable = await _nfseServiceRepository.GetNfseAsync(searchNf);

            return nfseEnumerable.MapTo<IEnumerable<Nfse>, IEnumerable<NfseResponse>>();
        }

        public async Task<IEnumerable<CompanyResponse>> GetEmpresasAsync(SearchCompanyRequest searchCompanyRequest)
        {
            var searchCompany = searchCompanyRequest.MapTo<SearchCompanyRequest, SearchCompany>();

            var companyEnumerable = await _nfseServiceRepository.GetEmpresasAsync(searchCompany);

            return companyEnumerable.MapTo<IEnumerable<Company>, IEnumerable<CompanyResponse>>();
        }

        public async Task<PdfResponse> GeneratePdfAsync(GeneratePdfRequest generatePdfRequest)
        {
            var generatePdf = generatePdfRequest.MapTo<GeneratePdfRequest, GeneratePdf>();

            var pdf = await _nfseServiceRepository.GeneratePdfAsync(generatePdf);

            return pdf.MapTo<Pdf, PdfResponse>();
        }
    }
}
