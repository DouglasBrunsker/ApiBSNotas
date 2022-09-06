using Brunsker.Bsnotas.Domain.Interfaces;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.WebApi.Repository.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.OracleAdapter.Repositories.RepositoryBase
{
    public class NfseServicoRepository : MapperProcedure, INfseServiceRepository
    {
        public NfseServicoRepository(IConfiguration config, ILogger<object> logger) : base(config, logger)
        {
        }

        public async Task<IEnumerable<SearchNfse>> GetTotalizadoresAsync(SearchNfse searchNfse) =>
           await ExecProcedureAsync<SearchNfse, SearchNfse>(searchNfse, "PKG_BS_NF_SERVICO2.PESQ_TOTALIZADORES");

        public async Task<IEnumerable<SearchNfse>> GetRecebidasDiaAsync(SearchNfse searchNfse) =>
           await ExecProcedureAsync<SearchNfse, SearchNfse>(searchNfse, "PKG_BS_NF_SERVICO2.PESQ_NFSE_RECEBIDAS_DIA");

        public async Task<IEnumerable<SearchNf>> GetNfseAsync(SearchNf searchNf) =>
           await ExecProcedureAsync<SearchNf, SearchNf>(searchNf, "PKG_BS_NF_SERVICO2.PESQ_NFSE");
    }
}
