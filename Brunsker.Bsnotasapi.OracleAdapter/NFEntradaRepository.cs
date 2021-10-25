using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brunsker.Bsnotas.Domain.Models;
using Dapper.Oracle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Brunsker.Bsnotasapi.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Interfaces;

namespace Brunsker.Bsnotas.OracleAdapter
{
    public class NFEntradaRepository : INFEntradaRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NFEntradaRepository> _logger;
        private readonly string _connectionString;

        public NFEntradaRepository(IConfiguration configuration, ILogger<NFEntradaRepository> logger)
        {
            _configuration = configuration;

            _logger = logger;

            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }
        public async Task<IEnumerable<EmpresasCliente>> BuscarEmpresasAsync(long seqCliente)
        {
            try
            {
                string sql = "pkg_bs_nf_entrada.PESQ_EMPRESAS";

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
        public async Task<Totalizadores> BuscarTotalizadoresAsync(DateTime? dataInicio, DateTime? dataFim, int seqCliente)
        {

            try
            {
                string sql = "pkg_bs_nf_entrada.PESQ_TOTALIZADORES";

                using (var conn = new OracleConnection(_connectionString))
                {

                    if (conn.State == ConnectionState.Closed) if (conn.State == ConnectionState.Closed) conn.Open();

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", seqCliente);

                    dynamicParameters.Add("pDATAINI", dataInicio);

                    dynamicParameters.Add("pDATAFIM", dataFim);

                    dynamicParameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conn.QueryFirstOrDefaultAsync<Totalizadores>(sql, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
        }
        public async Task<IEnumerable<TotalizadorNotasPorDia>> TotalNotasFiscaisEntradaRecebidasPorDia(long Vseq_Cliente, DateTime? dataIni, DateTime? dataFim)
        {
            try
            {
                string sql = "pkg_bs_nf_entrada.PESQ_NFENT_RECEBIDAS_DIA";

                using (var conn = new OracleConnection(_connectionString))
                {
                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", Vseq_Cliente);

                    dynamicParameters.Add("pDATAINI", dataIni);

                    dynamicParameters.Add("pDATAFIM", dataFim);

                    dynamicParameters.Add(name: ":CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conn.QueryAsync<TotalizadorNotasPorDia>(sql, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
        }
        public async Task<IEnumerable<NF>> BuscarNotasFiscaisEntradaAsync(ParametrosPesquisaNfEntrada pesquisa)
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


                dynamicParameters.Add("pSEQ_CLIENTE", pesquisa.seqCliente);
                dynamicParameters.Add("pDATAINI", pesquisa.DATAINI);
                dynamicParameters.Add("pDATAFIM", pesquisa.DATAFIM);
                dynamicParameters.Add("pCHAVENFE", pesquisa.CHAVENFE);
                dynamicParameters.Add("pNATUREZAOPER", pesquisa.NATUREZAOPER);
                dynamicParameters.Add("pNUMNOTA", pesquisa.NUMNOTA);
                dynamicParameters.Add("pCNPJEMITENTE", pesquisa.CNPJEMITENTE);
                dynamicParameters.Add("pNOMEEMITENTE", pesquisa.NOMEEMITENTE);
                dynamicParameters.Add("pCNPJDEST", pesquisa.CNPJDEST);
                dynamicParameters.Add("pNOMEDEST", pesquisa.NOMEDEST);
                dynamicParameters.Add("pDTENTINI", dataEntradaIni);
                dynamicParameters.Add("pDTENTFIM", dataEntradaFim);
                dynamicParameters.Add("pUF", pesquisa.UF);
                dynamicParameters.Add("pCFOP", pesquisa.CFOP);
                dynamicParameters.Add("pEXIBIRNFELIVROFISCAL", pesquisa.EXIBIRNFELIVROFISCAL == true ? 1 : 0);
                dynamicParameters.Add("pCARTACORRECAO", pesquisa.CARTACORRECAO == true ? 1 : 0);
                dynamicParameters.Add("pNFSEMENTRADAERP", pesquisa.NFSEMENTRADAERP == true ? 1 : 0);
                dynamicParameters.Add("pNOTASENTPREENT", pesquisa.NOTASENTPREENT == true ? 1 : 0);
                dynamicParameters.Add("pNOTASENTERP", pesquisa.NOTASENTERP == true ? 1 : 0);
                dynamicParameters.Add("pDEVOLUCAO", pesquisa.DEVOLUCAO == true ? 1 : 0);
                dynamicParameters.Add("pTRANSF", pesquisa.TRANSF == true ? 1 : 0);
                dynamicParameters.Add("pEXCETOTRANSF", pesquisa.EXIBIRNFELIVROFISCAL == true ? 1 : 0);
                dynamicParameters.Add("pSTATUSNFEAUTORI", pesquisa.STATUSNFEAUTORI == true ? 1 : 0);
                dynamicParameters.Add("pSTATUSNFECANC", pesquisa.STATUSNFECANC == true ? 1 : 0);
                dynamicParameters.Add("pSTATUSNFEDENEGADO", pesquisa.STATUSNFEDENEGADO == true ? 1 : 0);
                dynamicParameters.Add("pSTATUSMANIFNENHUMA", pesquisa.STATUSMANIFNENHUMA == true ? 1 : 0);
                dynamicParameters.Add("pSTATUSMANIFCIENCIA", pesquisa.STATUSMANIFCIENCIA == true ? 1 : 0);
                dynamicParameters.Add("pSTATUSMANIFCONFIRMADA", pesquisa.STATUSMANIFCONFIRMADA == true ? 1 : 0);
                dynamicParameters.Add("pSTATUSMANIFDESCONHECIDA", pesquisa.STATUSMANIFDESCONHECIDA == true ? 1 : 0);
                dynamicParameters.Add("pSTATUSMANIFNAOREALIZADA", pesquisa.STATUSMANIFNAOREALIZADA == true ? 1 : 0);
                dynamicParameters.Add("pEMPRESASCADASTRADAS", empresas);
                dynamicParameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conn.QueryAsync<NF>("pkg_bs_nf_entrada.PESQ_NFENT", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }
        public async Task<IEnumerable<CFOP>> BuscarCfopNotaFiscalEntradaAsync(long seqCliente)
        {
            try
            {
                string sql = "pkg_bs_nf_entrada.PESQ_CFOP";

                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) if (conn.State == ConnectionState.Closed) conn.Open();

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", seqCliente);

                    dynamicParameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conn.QueryAsync<CFOP>(sql, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
        }
        public async Task<IEnumerable<Fornecedores>> BuscarFornecedoresAsync(long seqCliente)
        {
            try
            {
                string sql = "pkg_bs_nf_entrada.PESQ_FORNECEDORES";

                using OracleConnection conn = new OracleConnection(_connectionString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                dynamicParameters.Add("pSEQ_CLIENTE", seqCliente);

                dynamicParameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conn.QueryAsync<Fornecedores>(sql, param: dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
        }
        public async Task<IEnumerable<DetalheNotaFiscalEntrada>> BuscarDetalhesNotaFiscalEntradaAsync(long seqCliente, string chaveNfe)
        {
            try
            {
                string sql = "pkg_bs_nf_entrada.PESQ_DETALHES_NFENT";

                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    OracleDynamicParameters dynamicParameters = new OracleDynamicParameters();

                    dynamicParameters.Add("pSEQ_CLIENTE", seqCliente);

                    dynamicParameters.Add("pCHAVENFE", chaveNfe);

                    dynamicParameters.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conn.QueryAsync<DetalheNotaFiscalEntrada>(sql, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
        }
        public async Task<string> SelectArquivoXml(string chave)
        {
            string sql = $@"SELECT T.ARQUIVO_XML FROM BSNT_ARQUIVOXML_NFE_ENTRADA t WHERE T.CHAVENFE = '{chave}'";

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
        public async Task<string> SelectArquivoXmlCCe(string chave)
        {
            string sql = $@"SELECT CCE.ARQUIVO_XML XML_CONTEUDO FROM BSNT_CCE_NFE CCE WHERE CCE.CHAVE = '{chave}'";

            try
            {
                using (var conn = new OracleConnection(_configuration.GetConnectionString("OracleConnectionOld")))
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
        public async Task<IEnumerable<TotalizadorNotasPorDia>> BuscarTotalizadoresGraficoAsync(FiltroTotalizadores filtro)
        {
            string sql = "pkg_bs_nf_entrada.PESQ_NFENT_RECEBIDAS_DIA";

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

                    var result = await conn.QueryAsync<TotalizadorNotasPorDia>(sql, parameters, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
        }
        public async Task<RecepcaoEvento> SelectRelacaoWebServices(int seq_cliente, string cnpj)
        {
            RecepcaoEvento recepcao = null;

            try
            {
                string sql = "pkg_webserv_insert_bsnotas.SELECT_RELACAO_WEB_SERVICES";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                    parms.Add("pSEQ_CLIENTE", seq_cliente);
                    parms.Add("pCNPJ", cnpj);

                    recepcao = await conn.QueryFirstOrDefaultAsync<RecepcaoEvento>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return recepcao;
        }
        public async Task ConfirmaManifestacao(string chave, string codigoEvento)
        {
            try
            {
                string sql = "pkg_webserv_insert_bsnotas.PROC_CONFIRMA_MANIFESTACAO";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pCODIGO_EVENTO", int.Parse(codigoEvento));
                    parms.Add("pCHAVE", chave);

                    await conn.ExecuteAsync(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<ResultadoValidacaoPreEntrada>> ValidaPreEntrada(ValidarPreEntrada validar)
        {
            IEnumerable<ResultadoValidacaoPreEntrada> result = null;

            try
            {
                string sql = "pkg_pre_entrada.VALIDAR_PREENTRADA";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pSEQ_CLIENTE", validar.SEQ_CLIENTE);
                    parms.Add("pCHAVE", validar.CHAVE);
                    parms.Add("pCODFILIAL", validar.CODFILIAL);
                    parms.Add("pNUMPED", validar.NUMPED);
                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    result = await conn.QueryAsync<ResultadoValidacaoPreEntrada>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return result;
        }
        public async Task ProcessaPreEntrada(ItensPedidoPre item)
        {
            try
            {
                string sql = "pkg_pre_entrada.PROC_INS_PCMOVPREENT";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pSEQ_CLIENTE", item.SEQ_CLIENTE);
                    parms.Add("pPREENTSEMVINCPED", item.PREENTSEMVINCULOPED);
                    parms.Add("pCHAVENFE", item.CHAVE);
                    parms.Add("pCODPROD", item.CODPROD == 0 ? null : item.CODPROD);
                    parms.Add("pNUMSEQ", item.NUMSEQ == 0 ? null : item.NUMSEQ);
                    parms.Add("pQTDE", item.QTPENTREGUE == 0 ? null : item.QTPENTREGUE);
                    parms.Add("pNUMPEDPREENT", item.NUMPED == 0 ? null : item.NUMPED);
                    parms.Add("pMSG", item.Mensagem);

                    await conn.ExecuteAsync(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
        }
        public async Task<IEnumerable<PedidoAssociado>> SelectPedidosAssociados(string chave, int seqCliente)
        {
            IEnumerable<PedidoAssociado> pedidos = null;
            try
            {
                string sql = "pkg_pre_entrada.PROC_ASSOCIA_PEDIDO";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pSEQ_CLIENTE", seqCliente);
                    parms.Add("pCHAVE", chave);
                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    pedidos = await conn.QueryAsync<PedidoAssociado>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return pedidos;
        }
        public async Task<IEnumerable<ItemPedido>> SelectItensPedido(PesquisaItensPedido pesq)
        {
            IEnumerable<ItemPedido> itens = null;

            try
            {
                string sql = "pkg_pre_entrada.PROC_EXIBIR_ITENSPED";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pCHAVE", pesq.CHAVE);
                    parms.Add("pSEQ_CLIENTE", pesq.SEQ_CLIENTE);
                    parms.Add("pNUMPED", pesq.NUMPED);
                    parms.Add("pCNPJEMITENTE", pesq.CNPJ);
                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    itens = await conn.QueryAsync<ItemPedido>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return itens;
        }
        public async Task<IEnumerable<object>> BuscarLivroFiscal(ParametroGeracaoLivroFiscal parametro)
        {
            IEnumerable<object> livro = null;
            try
            {
                string sql = "pkg_pre_entrada.pesq_gerarnflivrofiscal";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pchave", parametro.Chave);
                    parms.Add("pcodfornec", parametro.CodigoFornecedor);
                    parms.Add("pnumnota", parametro.NumeroNota);
                    parms.Add("pdtemissao", parametro.DataEmissao);
                    parms.Add("pseq_cliente", parametro.SeqCliente);
                    parms.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    await conn.QueryAsync<object>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return livro;
        }
        public async Task<IEnumerable<Contas>> SelectContas(long seqCliente)
        {
            IEnumerable<Contas> contas = null;

            try
            {
                string sql = $"SELECT * FROM BSNT_PCCONTAS_WINTHOR C WHERE C.SEQ_CLIENTE = {seqCliente}";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    contas = await conn.QueryAsync<Contas>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return contas;
        }
        public async Task<bool> RemovePreEntrada(string chave)
        {
            try
            {
                string sql = "pkg_pre_entrada.PROC_DELETE_PREENTRADA";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pCHAVE", chave);

                    await conn.ExecuteAsync(sql, parms, commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return false;
            }
        }
    }
}


