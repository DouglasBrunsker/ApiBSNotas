using Brunsker.Bsnotas.Application.Requests.GeneratePdf;
using Brunsker.Bsnotas.Application.Requests.Searchs;
using Brunsker.Bsnotas.Application.Responses.Company;
using Brunsker.Bsnotas.Application.Responses.Notas;
using Brunsker.Bsnotas.Application.Responses.Pdf;
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
        Task<PdfResponse> GeneratePdfAsync(GeneratePdfRequest generatePdfRequest);
        Task<byte[]> ExportaNfsePdfsAsync(IEnumerable<GeneratePdfRequest> generatePdfRequestList);
        Task<byte[]> ExportaNfseExcelAsync(IEnumerable<int> notasServicoIdEnumerable);
        Task<byte[]> ExportaNfseXmlsAsync(IEnumerable<int> notasServicoIdEnumerable);
    }
}
