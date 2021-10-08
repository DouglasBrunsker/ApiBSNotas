using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
using Dapper;
using Dapper.Oracle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace Brunsker.Bsnotasapi.OracleAdapter
{
    public class CteRepository : ICteRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CteRepository> _logger;
        private readonly string _connectionString;

        public CteRepository(IConfiguration configuration, ILogger<CteRepository> logger)
        {
            _configuration = configuration;

            _logger = logger;

            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }

        public async Task<IEnumerable<Cte>> BuscarCteEntradaAsync(ParametrosPesquisaCteEntrada pesquisa)
        {
            try
            {
                string empresas = null;

                DateTime? dataEntradaIni = null;
                DateTime? dataEntradaFim = null;

                if (pesquisa?.DTENT?.Length > 0)
                {
                    dataEntradaIni = pesquisa.DTENT[0];
                    dataEntradaFim = pesquisa.DTENT[1];
                }

                if (pesquisa?.EMPRESASCADASTRADAS?.Length > 0)
                {

                    for (int i = 0; i < pesquisa.EMPRESASCADASTRADAS.Length; i++)
                    {
                        empresas += pesquisa.EMPRESASCADASTRADAS[i] + ",";
                    }
                    empresas = empresas.Substring(0, empresas.LastIndexOf(','));
                }

                using OracleConnection conn = new OracleConnection(_connectionString);

                conn.Open();

                OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();


                dynamicParameters.Add("pSEQ_CLIENTE", pesquisa.SEQ_CLIENTE);
                dynamicParameters.Add("pDATAINI", pesquisa.DATAINI);
                dynamicParameters.Add("pDATAFIM", pesquisa.DATAFIM);
                dynamicParameters.Add("pUF", pesquisa.UF);
                dynamicParameters.Add("pNUMNOTA", pesquisa.NUMNOTA);                
                dynamicParameters.Add("pEXIBIRCTEMANIFACORDO", pesquisa.EXIBIRCTEMANIFACORDO == true ? 1 : 0);                
                dynamicParameters.Add("pEXIBIRCTECARTACORRECAO", pesquisa.EXIBIRCTECARTACORRECAO == true ? 1 : 0);                
                dynamicParameters.Add("pEXIBIRCTECLITOMADOR", pesquisa.EXIBIRCTECLITOMADOR == true ? 1 : 0);                
                dynamicParameters.Add("pSTATUSCTEAUTORI", pesquisa.STATUSCTEAUTORI == true ? 1 : 0);                
                dynamicParameters.Add("pSTATUSCTECANC", pesquisa.STATUSCTECANC == true ? 1 : 0);                
                dynamicParameters.Add("pSTATUSCTEDENEGADO", pesquisa.STATUSCTEDENEGADO == true ? 1 : 0);                
                dynamicParameters.Add("pSTATUSMANIFDESACORDO", pesquisa.STATUSMANIFDESACORDO == true ? 1 : 0);
                dynamicParameters.Add("pNATUREZAOPER", pesquisa.NATUREZAOPER);                
                dynamicParameters.Add("pCHAVECTE", pesquisa.CHAVECTE);                
                dynamicParameters.Add("pCNPJEMITENTE", pesquisa.CNPJEMITENTE);                
                dynamicParameters.Add("pNOMEEMITENTE", pesquisa.NOMEEMITENTE);                
                dynamicParameters.Add("pCNPJDEST", pesquisa.CNPJDEST);                
                dynamicParameters.Add("pNOMEDEST", pesquisa.NOMEDEST);                
                dynamicParameters.Add("pEMPRESASCADASTRADAS", empresas);
                dynamicParameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conn.QueryAsync<Cte>("pkg_bs_cte_entrada.PESQ_CTENT", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }
    }
}