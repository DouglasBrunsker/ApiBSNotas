using Brunsker.Bsnotas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.Domain.Interfaces
{
    public interface INfseServiceRepository
    {
        Task<IEnumerable<Totalizador>> GetTotalizadoresAsync(SearchNfse searchNfse);
        Task<IEnumerable<NotasDia>> GetRecebidasDiaAsync(SearchNfse searchNfse);
        Task<IEnumerable<Nfse>> GetNfseAsync(SearchNf searchNf);
        Task<IEnumerable<Company>> GetEmpresasAsync(SearchCompany searchCompany);
        Task<string> GetArquivoXmlBySeqArquivoXmlNfse(int seqArquivoXmlNfse);
    }
}
