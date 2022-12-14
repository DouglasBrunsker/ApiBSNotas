using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
using Dapper;
using Dapper.Oracle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace Brunsker.Bsnotas.OracleAdapter.Repositories
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

        public async Task<IEnumerable<Cte>> BuscarCteAsync(ParametrosPesquisaCte pesquisa)
        {
            try
            {
                string empresas = null;

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
                dynamicParameters.Add("pNUMNOTA", pesquisa.NUMCTE);
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
                dynamicParameters.Add("pCNPJDEST", pesquisa.CNPJTOMADOR);
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
        public async Task<IEnumerable<EmpresasCliente>> BuscarEmpresasAsync(long seqCliente)
        {
            try
            {
                string sql = "pkg_bs_cte_entrada.PESQ_EMPRESAS";

                using OracleConnection conn = new OracleConnection(_connectionString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                dynamicParameters.Add("pSEQ_CLIENTE", seqCliente);

                dynamicParameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conn.QueryAsync<EmpresasCliente>(sql, param: dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
        }
        public async Task<TotalizadoresCte> BuscarTotalizadoresCteAsync(DateTime? dataInicio, DateTime? dataFim, int seqCliente)
        {
            try
            {
                string sql = "pkg_bs_cte_entrada.PESQ_TOTALIZADORES";

                using (var conn = new OracleConnection(_connectionString))
                {

                    if (conn.State == ConnectionState.Closed) if (conn.State == ConnectionState.Closed) conn.Open();

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", seqCliente);

                    dynamicParameters.Add("pDATAINI", dataInicio);

                    dynamicParameters.Add("pDATAFIM", dataFim);

                    dynamicParameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conn.QueryFirstAsync<TotalizadoresCte>(sql, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
        }
        public async Task<IEnumerable<TotalizadorCtePorDia>> BuscarTotalizadoresGraficoAsync(FiltroTotalizadores filtro)
        {
            string sql = "pkg_bs_cte_entrada.PESQ_CTE_RECEBIDAS_DIA";

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parameters = new OracleDynamicParameters();

                    parameters.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parameters.Add("pDATAINI", filtro.DataInicial);
                    parameters.Add("pDATAFIM", filtro.DataFinal);
                    parameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    var result = await conn.QueryAsync<TotalizadorCtePorDia>(sql, parameters, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
        }
        public async Task<IEnumerable<NfeVinculadasCTe>> BuscarNFeVinculadasCTeAsync(NfeVinculadasCTe pesquisa)
        {
            string sql = "pkg_bs_cte_entrada.PESQ_NFES_VINCULADAS_CTE";

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parameters = new OracleDynamicParameters();

                    parameters.Add("pSEQ_CLIENTE", pesquisa.SEQ_CLIENTE);
                    parameters.Add("pCHAVECTE", pesquisa.CHAVECTE);
                    parameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    var result = await conn.QueryAsync<NfeVinculadasCTe>(sql, parameters, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
        }
        public async Task<string> SelectArquivoXml(string chave)
        {
            string sql = $@"SELECT T.ARQUIVO_XML FROM BSNT_ARQUIVOXML_CTE_ENTRADA t WHERE T.CHAVECTE = '{chave}'";

            try
            {
                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var result = await conn.QueryFirstOrDefaultAsync<string>(sql);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
        }

        public async Task<RecepcaoEventoCte> SelectRelacaoWebServices(int seq_cliente, string cnpj, int tpAmb, int codUf)
        {
            RecepcaoEventoCte recepcao = null;

            try
            {
                string sql = "pkg_webserv_insert_bsnotas.SELECT_RELACAO_WS_CTE";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                    parms.Add("pSEQ_CLIENTE", seq_cliente);
                    parms.Add("pCNPJ", cnpj);
                    parms.Add("pTPAMB", tpAmb);
                    parms.Add("pCODUF", codUf);

                    recepcao = await conn.QueryFirstOrDefaultAsync<RecepcaoEventoCte>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return recepcao;
        }

        public async Task ConfirmaManifestacaoCte(string cStat, string nProt, string chCTe, int seqCliente, string xMotivo, string tpEvento)
        {
            try
            {
                string sql = "pkg_bs_cte_entrada.CONFIRMA_MANIFESTACAO_CTE";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pCODSTATUS", cStat);
                    parms.Add("pNPROTOCOLO", nProt);
                    parms.Add("pCHCTE", chCTe);
                    parms.Add("pSEQCLIENTE", seqCliente);
                    parms.Add("pLOG", xMotivo);
                    parms.Add("pTPEVENTO", int.Parse(tpEvento));

                    await conn.ExecuteAsync(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task LogErroManifestacaoCte(string cStat, string xMotivo, string chCTe, int seqCliente)
        {
            try
            {
                string sql = "pkg_bs_cte_entrada.ERRO_MANIFESTACAO_CTE";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pCODSTATUS", cStat);
                    parms.Add("pCHCTE", chCTe);
                    parms.Add("pSEQCLIENTE", seqCliente);
                    parms.Add("pLOG", xMotivo);

                    await conn.ExecuteAsync(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}