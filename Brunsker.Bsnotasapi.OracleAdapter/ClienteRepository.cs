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
                using (var conexao = new OracleConnection(_connectionString))
                {

                    var parametros= new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parametros.Add("pNOMECLIENTE", filtro.Nome);
                    parametros.Add("pCNPJ", filtro.Cnpj);
                    parametros.Add("pSTATUSBLOQ", filtro.Bloqueio == "S" ? 1 : 2);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    clientes = await conexao.QueryAsync<Cliente>("pkg_bs_consultas.CONSULTAR_CLIENTES", parametros, commandType: CommandType.StoredProcedure);
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