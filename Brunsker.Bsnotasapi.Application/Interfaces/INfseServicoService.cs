using Brunsker.Bsnotas.Application.Requests.SearchCompany;
using Brunsker.Bsnotas.Application.Requests.SearchNf;
using Brunsker.Bsnotas.Application.Requests.SearchNfse;
using Brunsker.Bsnotas.Application.Responses.Company;
using Brunsker.Bsnotas.Application.Responses.Nfse;
using Brunsker.Bsnotas.Application.Responses.NotasDia;
using Brunsker.Bsnotas.Application.Responses.Totalizador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.Application.Interfaces
{
    public interface INfseServicoService
    {
        Task<IEnumerable<TotalizadorResponse>> GetTotalizadoresAsync(SearchNfseRequest searchNfseRequest);
        Task<IEnumerable<NotasDiaResponse>> GetRecebidasDiaAsync(SearchNfseRequest searchNfseRequest);
        Task<IEnumerable<NfseResponse>> GetNfseAsync(SearchNfRequest searchNfRequest);
        Task<IEnumerable<CompanyResponse>> GetEmpresasAsync(SearchCompanyRequest searchCompanyRequest);
    }
}
