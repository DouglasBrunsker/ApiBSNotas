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
                using OracleConnection conexao = new OracleConnection(_connectionString);


                OracleDynamicParameters parametros = new OracleDynamicParameters();

                parametros.Add("pSEQ_CLIENTE", seqCliente);

                parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conexao.QueryAsync<EmpresasCliente>
                    ("pkg_bs_nf_entrada.PESQ_EMPRESAS",
                    parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {

                    OracleDynamicParameters parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", seqCliente);

                    parametros.Add("pDATAINI", dataInicio);

                    parametros.Add("pDATAFIM", dataFim);

                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conexao.QueryFirstOrDefaultAsync<Totalizadores>
                        ("pkg_bs_nf_entrada.PESQ_TOTALIZADORES",
                        parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    OracleDynamicParameters parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", Vseq_Cliente);

                    parametros.Add("pDATAINI", dataIni);

                    parametros.Add("pDATAFIM", dataFim);

                    parametros.Add(name: ":CUR_OUT", dbType: OracleMappingType.RefCursor,
                        direction: ParameterDirection.Output);

                    return await conexao.QueryAsync<TotalizadorNotasPorDia>
                        ("pkg_bs_nf_entrada.PESQ_NFENT_RECEBIDAS_DIA",
                        param: parametros, commandType: CommandType.StoredProcedure);
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

                using OracleConnection conexao = new OracleConnection(_connectionString);

                conexao.Open();

                OracleDynamicParameters parametros = new OracleDynamicParameters();


                parametros.Add("pSEQ_CLIENTE", pesquisa.seqCliente);
                parametros.Add("pDATAINI", pesquisa.DATAINI);
                parametros.Add("pDATAFIM", pesquisa.DATAFIM);
                parametros.Add("pCHAVENFE", pesquisa.CHAVENFE);
                parametros.Add("pNATUREZAOPER", pesquisa.NATUREZAOPER);
                parametros.Add("pNUMNOTA", pesquisa.NUMNOTA);
                parametros.Add("pCNPJEMITENTE", pesquisa.CNPJEMITENTE);
                parametros.Add("pNOMEEMITENTE", pesquisa.NOMEEMITENTE);
                parametros.Add("pCNPJDEST", pesquisa.CNPJDEST);
                parametros.Add("pNOMEDEST", pesquisa.NOMEDEST);
                parametros.Add("pDTENTINI", dataEntradaIni);
                parametros.Add("pDTENTFIM", dataEntradaFim);
                parametros.Add("pUF", pesquisa.UF);
                parametros.Add("pCFOP", pesquisa.CFOP);
                parametros.Add("pEXIBIRNFELIVROFISCAL", pesquisa.EXIBIRNFELIVROFISCAL == true ? 1 : 0);
                parametros.Add("pCARTACORRECAO", pesquisa.CARTACORRECAO == true ? 1 : 0);
                parametros.Add("pNFSEMENTRADAERP", pesquisa.NFSEMENTRADAERP == true ? 1 : 0);
                parametros.Add("pNOTASENTPREENT", pesquisa.NOTASENTPREENT == true ? 1 : 0);
                parametros.Add("pNOTASENTERP", pesquisa.NOTASENTERP == true ? 1 : 0);
                parametros.Add("pDEVOLUCAO", pesquisa.DEVOLUCAO == true ? 1 : 0);
                parametros.Add("pTRANSF", pesquisa.TRANSF == true ? 1 : 0);
                parametros.Add("pEXCETOTRANSF", pesquisa.EXIBIRNFELIVROFISCAL == true ? 1 : 0);
                parametros.Add("pSTATUSNFEAUTORI", pesquisa.STATUSNFEAUTORI == true ? 1 : 0);
                parametros.Add("pSTATUSNFECANC", pesquisa.STATUSNFECANC == true ? 1 : 0);
                parametros.Add("pSTATUSNFEDENEGADO", pesquisa.STATUSNFEDENEGADO == true ? 1 : 0);
                parametros.Add("pSTATUSMANIFNENHUMA", pesquisa.STATUSMANIFNENHUMA == true ? 1 : 0);
                parametros.Add("pSTATUSMANIFCIENCIA", pesquisa.STATUSMANIFCIENCIA == true ? 1 : 0);
                parametros.Add("pSTATUSMANIFCONFIRMADA", pesquisa.STATUSMANIFCONFIRMADA == true ? 1 : 0);
                parametros.Add("pSTATUSMANIFDESCONHECIDA", pesquisa.STATUSMANIFDESCONHECIDA == true ? 1 : 0);
                parametros.Add("pSTATUSMANIFNAOREALIZADA", pesquisa.STATUSMANIFNAOREALIZADA == true ? 1 : 0);
                parametros.Add("pNFUSOECONSUMO", pesquisa.NFUSOECONSUMO == true ? 1 : 0);
                parametros.Add("pEMPRESASCADASTRADAS", empresas);
                parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conexao.QueryAsync<NF>("pkg_bs_nf_entrada.PESQ_NFENT",
                    parametros, commandType: CommandType.StoredProcedure);
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
                using (OracleConnection conexao = new OracleConnection(_connectionString))
                {
                    OracleDynamicParameters parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", seqCliente);

                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conexao.QueryAsync<CFOP>("pkg_bs_nf_entrada.PESQ_CFOP",
                        parametros, commandType: CommandType.StoredProcedure);
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
                using OracleConnection conexao = new OracleConnection(_connectionString);

                OracleDynamicParameters parametros = new OracleDynamicParameters();

                parametros.Add("pSEQ_CLIENTE", seqCliente);

                parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                return await conexao.QueryAsync<Fornecedores>("pkg_bs_nf_entrada.PESQ_FORNECEDORES", parametros, commandType: CommandType.StoredProcedure);
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
                using (OracleConnection conexao = new OracleConnection(_connectionString))
                {
                    OracleDynamicParameters parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", seqCliente);

                    parametros.Add("pCHAVENFE", chaveNfe);

                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    return await conexao.QueryAsync<DetalheNotaFiscalEntrada>
                        ("pkg_bs_nf_entrada.PESQ_DETALHES_NFENT", 
                        parametros, commandType: CommandType.StoredProcedure);
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
            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {

                    var result = await conexao.QueryFirstOrDefaultAsync<string>
                    ($@"SELECT T.ARQUIVO_XML FROM BSNT_ARQUIVOXML_NFE_ENTRADA t WHERE T.CHAVENFE = '{chave}'");

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
            try
            {
                using (var conexao = new OracleConnection(_configuration.GetConnectionString("OracleConnectionOld")))
                {

                    var result = await conexao.QueryFirstOrDefaultAsync<string>
                        ($@"SELECT CCE.ARQUIVO_XML XML_CONTEUDO FROM BSNT_CCE_NFE CCE WHERE CCE.CHAVE = '{chave}'");

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
            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parametros.Add("pDATAINI", filtro.DataInicial);
                    parametros.Add("pDATAFIM", filtro.DataFinal);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    var result = await conexao.QueryAsync<TotalizadorNotasPorDia>
                        ("pkg_bs_nf_entrada.PESQ_NFENT_RECEBIDAS_DIA", parametros, commandType: CommandType.StoredProcedure);

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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                    parametros.Add("pSEQ_CLIENTE", seq_cliente);
                    parametros.Add("pCNPJ", cnpj);

                    recepcao = await conexao.QueryFirstOrDefaultAsync<RecepcaoEvento>
                        ("pkg_webserv_insert_bsnotas.SELECT_RELACAO_WEB_SERVICES", 
                        parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pCODIGO_EVENTO", int.Parse(codigoEvento));
                    parametros.Add("pCHAVE", chave);

                    await conexao.ExecuteAsync("pkg_webserv_insert_bsnotas.PROC_CONFIRMA_MANIFESTACAO",
                        parametros, commandType: CommandType.StoredProcedure);
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

                using (var conexao = new OracleConnection(_connectionString))
                {

                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", validar.SEQ_CLIENTE);
                    parametros.Add("pCHAVE", validar.CHAVE);
                    parametros.Add("pCODFILIAL", validar.CODFILIAL);
                    parametros.Add("pNUMPED", validar.NUMPED);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    result = await conexao.QueryAsync<ResultadoValidacaoPreEntrada>
                        ("pkg_pre_entrada.VALIDAR_PREENTRADA", 
                        parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {

                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", item.SEQ_CLIENTE);
                    parametros.Add("pPREENTSEMVINCPED", item.PREENTSEMVINCULOPED);
                    parametros.Add("pCHAVENFE", item.CHAVE);
                    parametros.Add("pCODPROD", item.CODPROD == 0 ? null : item.CODPROD);
                    parametros.Add("pNUMSEQ", item.NUMSEQ == 0 ? null : item.NUMSEQ);
                    parametros.Add("pQTDE", item.QTPENTREGUE == 0 ? null : item.QTPENTREGUE);
                    parametros.Add("pNUMPEDPREENT", item.NUMPED == 0 ? null : item.NUMPED);
                    parametros.Add("pMSG", item.Mensagem);
                    parametros.Add("pCODFORNEC", item.codFornec == 0 ? null : item.codFornec);

                    await conexao.ExecuteAsync("pkg_pre_entrada.PROC_INS_PCMOVPREENT",
                        parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", seqCliente);
                    parametros.Add("pCHAVE", chave);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    pedidos = await conexao.QueryAsync<PedidoAssociado>
                        ("pkg_pre_entrada.PROC_ASSOCIA_PEDIDO", 
                        parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pCHAVE", pesq.CHAVE);
                    parametros.Add("pSEQ_CLIENTE", pesq.SEQ_CLIENTE);
                    parametros.Add("pNUMPED", pesq.NUMPED);
                    parametros.Add("pCNPJEMITENTE", pesq.CNPJ);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    itens = await conexao.QueryAsync<ItemPedido>("pkg_pre_entrada.PROC_EXIBIR_ITENSPED",
                        parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pchave", parametro.Chave);
                    parametros.Add("pcodfornec", parametro.CodigoFornecedor);
                    parametros.Add("pnumnota", parametro.NumeroNota);
                    parametros.Add("pdtemissao", parametro.DataEmissao);
                    parametros.Add("pseq_cliente", parametro.SeqCliente);
                    parametros.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    await conexao.QueryAsync<object>
                        ("pkg_pre_entrada.pesq_gerarnflivrofiscal", 
                        parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {

                    contas = await conexao.QueryAsync<Contas>
                        ($"SELECT * FROM BSNT_PCCONTAS_WINTHOR C WHERE C.SEQ_CLIENTE = {seqCliente}");
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pCHAVE", chave);

                    await conexao.ExecuteAsync("pkg_pre_entrada.PROC_DELETE_PREENTRADA",
                        parametros, commandType: CommandType.StoredProcedure);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return false;
            }
        }

        public async Task<IEnumerable<FornecedoresAssociados>> SelectFornecedoresAssociados(long seqCliente, string cnpj)
        {
            IEnumerable<FornecedoresAssociados> fornecedores = null;

            try
            {

                using (var conexao = new OracleConnection(_connectionString))
                {

                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", seqCliente);
                    parametros.Add("pCNPJEMITENTE", cnpj);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    fornecedores = await conexao.QueryAsync<FornecedoresAssociados>
                        ("pkg_pre_entrada.PROC_EXIBIR_FORNEC", parametros, 
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return fornecedores;
        }
    }
}


