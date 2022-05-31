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

                using OracleConnection conexao = new OracleConnection(_connectionString);

                conexao.Open();

                OracleDynamicParameters parametros = new OracleDynamicParameters();


                parametros.Add("pSEQ_CLIENTE", pesquisa.SEQ_CLIENTE);
                parametros.Add("pDATAINI", pesquisa.DATAINI);
                parametros.Add("pDATAFIM", pesquisa.DATAFIM);
                parametros.Add("pUF", pesquisa.UF);
                parametros.Add("pNUMNOTA", pesquisa.NUMCTE);
                parametros.Add("pEXIBIRCTEMANIFACORDO", pesquisa.EXIBIRCTEMANIFACORDO == true ? 1 : 0);
                parametros.Add("pEXIBIRCTECARTACORRECAO", pesquisa.EXIBIRCTECARTACORRECAO == true ? 1 : 0);
                parametros.Add("pEXIBIRCTECLITOMADOR", pesquisa.EXIBIRCTECLITOMADOR == true ? 1 : 0);
                parametros.Add("pSTATUSCTEAUTORI", pesquisa.STATUSCTEAUTORI == true ? 1 : 0);
                parametros.Add("pSTATUSCTECANC", pesquisa.STATUSCTECANC == true ? 1 : 0);
                parametros.Add("pSTATUSCTEDENEGADO", pesquisa.STATUSCTEDENEGADO == true ? 1 : 0);
                parametros.Add("pSTATUSMANIFDESACORDO", pesquisa.STATUSMANIFDESACORDO == true ? 1 : 0);
                parametros.Add("pNATUREZAOPER", pesquisa.NATUREZAOPER);
                parametros.Add("pCHAVECTE", pesquisa.CHAVECTE);
                parametros.Add("pCNPJEMITENTE", pesquisa.CNPJEMITENTE);
                parametros.Add("pNOMEEMITENTE", pesquisa.NOMEEMITENTE);
                parametros.Add("pCNPJDEST", pesquisa.CNPJTOMADOR);
                parametros.Add("pNOMEDEST", pesquisa.NOMEDEST);
                parametros.Add("pEMPRESASCADASTRADAS", empresas);
                parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conexao.QueryAsync<Cte>("pkg_bs_cte_entrada.PESQ_CTENT", parametros, commandType: CommandType.StoredProcedure);
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
                using OracleConnection conexao = new OracleConnection(_connectionString);

                OracleDynamicParameters parametros = new OracleDynamicParameters();

                parametros.Add("pSEQ_CLIENTE", seqCliente);

                parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conexao.QueryAsync<EmpresasCliente>("pkg_bs_cte_entrada.PESQ_EMPRESAS", parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    OracleDynamicParameters parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", seqCliente);

                    parametros.Add("pDATAINI", dataInicio);

                    parametros.Add("pDATAFIM", dataFim);

                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conexao.QueryFirstAsync<TotalizadoresCte>("pkg_bs_cte_entrada.PESQ_TOTALIZADORES", parametros, commandType: CommandType.StoredProcedure);
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
            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parametros.Add("pDATAINI", filtro.DataInicial);
                    parametros.Add("pDATAFIM", filtro.DataFinal);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    var result = await conexao.QueryAsync<TotalizadorCtePorDia>("pkg_bs_cte_entrada.PESQ_CTE_RECEBIDAS_DIA", parametros, commandType: CommandType.StoredProcedure);

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
            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {

                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", pesquisa.SEQ_CLIENTE);
                    parametros.Add("pCHAVECTE", pesquisa.CHAVECTE);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    var result = await conexao.QueryAsync<NfeVinculadasCTe>("pkg_bs_cte_entrada.PESQ_NFES_VINCULADAS_CTE", parametros, commandType: CommandType.StoredProcedure);

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
            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var result = await conexao.QueryFirstOrDefaultAsync<string>
                        ($@"SELECT T.ARQUIVO_XML FROM BSNT_ARQUIVOXML_CTE_ENTRADA t WHERE T.CHAVECTE = '{chave}'");

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
                using (var conexao = new OracleConnection(_connectionString))
                {                  
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                    parametros.Add("pSEQ_CLIENTE", seq_cliente);
                    parametros.Add("pCNPJ", cnpj);
                    parametros.Add("pTPAMB", tpAmb);
                    parametros.Add("pCODUF", codUf);

                    recepcao = await conexao.QueryFirstOrDefaultAsync<RecepcaoEventoCte>
                        ("pkg_webserv_insert_bsnotas.SELECT_RELACAO_WS_CTE", parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pCODSTATUS", cStat);
                    parametros.Add("pNPROTOCOLO", nProt);
                    parametros.Add("pCHCTE", chCTe);
                    parametros.Add("pSEQCLIENTE", seqCliente);
                    parametros.Add("pLOG", xMotivo);
                    parametros.Add("pTPEVENTO", int.Parse(tpEvento));

                    await conexao.ExecuteAsync("pkg_bs_cte_entrada.CONFIRMA_MANIFESTACAO_CTE", parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pCODSTATUS", cStat);
                    parametros.Add("pCHCTE", chCTe);
                    parametros.Add("pSEQCLIENTE", seqCliente);
                    parametros.Add("pLOG", xMotivo);

                    await conexao.ExecuteAsync("pkg_bs_cte_entrada.ERRO_MANIFESTACAO_CTE", parametros, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}