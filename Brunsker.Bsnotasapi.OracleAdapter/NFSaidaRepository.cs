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
        public async Task<IEnumerable<NotaFiscalSaida>> BuscaNotas(FiltroBuscaNotasSaida filtro)
        {
            IEnumerable<NotaFiscalSaida> notas = null;

            try
            {
                string sql = "pkg_bs_nf_saida.pesq_nfsaida";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pseq_cliente", filtro.SeqCliente);
                    parms.Add("pdataini", filtro.DataInicial);
                    parms.Add("pdatafim", filtro.DataFinal);
                    parms.Add("pchavenfe", filtro.Chave);
                    parms.Add("pnaturezaoper", filtro.NaturezaOperacao);
                    parms.Add("pnumnota", filtro.NumeroNota == 0 ? null : filtro.NumeroNota);
                    parms.Add("pcnpjemitente", filtro.CnpjEmitente);
                    parms.Add("pdtentini", null);
                    parms.Add("pdtentfim", null);
                    parms.Add("cur_out", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    notas = await conn.QueryAsync<NotaFiscalSaida>(sql, parms, commandType: CommandType.StoredProcedure);
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
        public async Task<Totalizadores> BuscarTotalizador(FiltroTotalizadores filtro)
        {

            Totalizadores totalizador = null;

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

                    totalizador = await conn.QueryFirstOrDefaultAsync<Totalizadores>(sql, param: parms, commandType: CommandType.StoredProcedure);
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
    }
}