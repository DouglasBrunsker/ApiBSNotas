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
                string sql = "pkg_bs_nf_saida.pesq_nfsaida";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parms.Add("pDATAINI", filtro.DataInicial);
                    parms.Add("pDATAFIM", filtro.DataFinal);
                    parms.Add("pCHAVENFE", filtro.Chave);
                    parms.Add("pNATUREZAOPER", filtro.NaturezaOperacao);
                    parms.Add("pNUMNOTA", filtro.NumeroNota == 0 ? null : filtro.NumeroNota);
                    parms.Add("pCNPJEMITENTE", filtro.CnpjEmitente);
                    parms.Add("pDEVOLUCAO", filtro.Devolucao == true ? 1 : 0);
                    parms.Add("pTRANSF", filtro.Transferencia == true ? 1 : 0);
                    parms.Add("pSTATUSNFEAUTORI", filtro.Autorizadas == true ? 1 : 0);
                    parms.Add("pSTATUSNFECANC", filtro.Canceladas == true ? 1 : 0);
                    parms.Add("pSTATUSNFEDENEGADO", filtro.Denegadas == true ? 1 : 0);

                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    notas = await conn.QueryAsync<NF>(sql, parms, commandType: CommandType.StoredProcedure);
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
                string sql = "pkg_bs_nf_saida.pesq_empresas";

                using OracleConnection conn = new OracleConnection(_connectionString);

                if (conn.State == ConnectionState.Closed) conn.Open();

                var parms = new OracleDynamicParameters();

                parms.Add("pseq_cliente", seqCliente);

                parms.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                empresas = await conn.QueryAsync<EmpresasCliente>(sql, param: parms, commandType: CommandType.StoredProcedure);
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
                string sql = "pkg_bs_nf_saida.pesq_nfsaida_recebidas_dia";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pseq_cliente", filtro.SeqCliente);

                    parms.Add("pdataini", filtro.DataInicial);

                    parms.Add("pdatafim", filtro.DataFinal);

                    parms.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    totalizador = await conn.QueryAsync<TotalizadorNotasPorDia>(sql, param: parms, commandType: CommandType.StoredProcedure);
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
                string sql = "pkg_bs_nf_saida.pesq_totalizadores";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pseq_cliente", filtro.SeqCliente);

                    parms.Add("pdataini", filtro.DataInicial);

                    parms.Add("pdatafim", filtro.DataFinal);

                    parms.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    totalizador = await conn.QueryFirstOrDefaultAsync<TotalizadorSaida>(sql, param: parms, commandType: CommandType.StoredProcedure);
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
                string sql = "pkg_bs_nf_saida.pesq_cfop";

                using (OracleConnection conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pseq_cliente", seqCliente);

                    parms.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    cfops = await conn.QueryAsync<CFOP>(sql, param: parms, commandType: CommandType.StoredProcedure);
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
            string sql = $@"SELECT T.ARQUIVO_XML FROM BSNT_ARQUIVOXML_NFE_SAIDA T WHERE T.CHAVENFE = '{chave}'";

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
    }
}