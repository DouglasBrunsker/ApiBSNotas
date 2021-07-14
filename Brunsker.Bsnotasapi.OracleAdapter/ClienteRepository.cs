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
    public class ClienteRepository : IClienteRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClienteRepository> _logger;
        private readonly string _connectionString;

        public ClienteRepository(IConfiguration configuration, ILogger<ClienteRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }
        public async Task<IEnumerable<Cliente>> SelectClientes(FiltroPesquisaClientes filtro)
        {
            IEnumerable<Cliente> clientes = null;
            try
            {
                string sql = "pkg_bs_consultas.CONSULTAR_CLIENTES";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parms.Add("pNOMECLIENTE", filtro.NomeCliente);
                    parms.Add("pCNPJ", filtro.Cnpj);
                    parms.Add("pSTATUSBLOQ", filtro.StatusBloqueio);
                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    clientes = await conn.QueryAsync<Cliente>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return clientes;
        }
    }
}