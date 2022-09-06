using Brunsker.Bsnotas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.Domain.Interfaces
{
    public interface INfseServiceRepository
    {
        Task<IEnumerable<SearchNfse>> GetTotalizadoresAsync(SearchNfse searchNfse);
        Task<IEnumerable<SearchNfse>> GetRecebidasDiaAsync(SearchNfse searchNfse);
        Task<IEnumerable<SearchNf>> GetNfseAsync(SearchNf searchNf);
    }
}
