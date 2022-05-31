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
    public class NFSaidaRepository : INFSaidaRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NFSaidaRepository> _logger;
        private readonly string _connectionString;

        public NFSaidaRepository(IConfiguration configuration, ILogger<NFSaidaRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }
        public async Task<IEnumerable<NF>> BuscaNotas(FiltroBuscaNotasSaida filtro)
        {
            IEnumerable<NF> notas = null;

            try
            {

                using (var conexao = new OracleConnection(_connectionString))
                {

                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parametros.Add("pDATAINI", filtro.DataInicial);
                    parametros.Add("pDATAFIM", filtro.DataFinal);
                    parametros.Add("pCHAVENFE", filtro.Chave);
                    parametros.Add("pNATUREZAOPER", filtro.NaturezaOperacao);
                    parametros.Add("pNUMNOTA", filtro.NumeroNota == 0 ? null : filtro.NumeroNota);
                    parametros.Add("pCNPJEMITENTE", filtro.CnpjEmitente);
                    parametros.Add("pDEVOLUCAO", filtro.Devolucao == true ? 1 : 0);
                    parametros.Add("pTRANSF", filtro.Transferencia == true ? 1 : 0);
                    parametros.Add("pSTATUSNFEAUTORI", filtro.Autorizadas == true ? 1 : 0);
                    parametros.Add("pSTATUSNFECANC", filtro.Canceladas == true ? 1 : 0);
                    parametros.Add("pSTATUSNFEDENEGADO", filtro.Denegadas == true ? 1 : 0);

                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    notas = await conexao.QueryAsync<NF>("pkg_bs_nf_saida.pesq_nfsaida",
                        parametros, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return notas;
        }
        public async Task<IEnumerable<EmpresasCliente>> BuscarEmpresas(long seqCliente)
        {
            IEnumerable<EmpresasCliente> empresas = null;

            try
            {

                using OracleConnection conexao = new OracleConnection(_connectionString);

                var parametros = new OracleDynamicParameters();

                parametros.Add("pseq_cliente", seqCliente);

                parametros.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                empresas = await conexao.QueryAsync<EmpresasCliente>("pkg_bs_nf_saida.pesq_empresas",
                    parametros, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return empresas;
        }
        public async Task<IEnumerable<TotalizadorNotasPorDia>> TotalizadorNotasEmitidasDia(FiltroTotalizadores filtro)
        {
            IEnumerable<TotalizadorNotasPorDia> totalizador = null;

            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pseq_cliente", filtro.SeqCliente);

                    parametros.Add("pdataini", filtro.DataInicial);

                    parametros.Add("pdatafim", filtro.DataFinal);

                    parametros.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    totalizador = await conexao.QueryAsync<TotalizadorNotasPorDia>
                        ("pkg_bs_nf_saida.pesq_nfsaida_recebidas_dia", parametros, 
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return totalizador;
        }
        public async Task<TotalizadorSaida> BuscarTotalizador(FiltroTotalizadores filtro)
        {

            TotalizadorSaida totalizador = null;

            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pseq_cliente", filtro.SeqCliente);

                    parametros.Add("pdataini", filtro.DataInicial);

                    parametros.Add("pdatafim", filtro.DataFinal);

                    parametros.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    totalizador = await conexao.QueryFirstOrDefaultAsync<TotalizadorSaida>
                        ("pkg_bs_nf_saida.pesq_totalizadores", parametros, 
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return totalizador;
        }
        public async Task<IEnumerable<CFOP>> BuscarCFOPs(long seqCliente)
        {
            IEnumerable<CFOP> cfops = null;

            try
            {
                using (OracleConnection conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pseq_cliente", seqCliente);

                    parametros.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    cfops = await conexao.QueryAsync<CFOP>("pkg_bs_nf_saida.pesq_cfop", parametros, 
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return cfops;
        }

        public async Task<string> SelectArquivoXml(string chave)
        {
            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var result = await conexao.QueryFirstOrDefaultAsync<string>
                        ($@"SELECT T.ARQUIVO_XML FROM BSNT_ARQUIVOXML_NFE_SAIDA T WHERE T.CHAVENFE = '{chave}'");

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
    }
}