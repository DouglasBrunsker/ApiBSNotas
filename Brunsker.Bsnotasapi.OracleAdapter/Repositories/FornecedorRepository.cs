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

namespace Brunsker.Bsnotas.OracleAdapter.Repositories
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FornecedorRepository> _logger;
        private readonly string _connectionString;

        public FornecedorRepository(IConfiguration configuration, ILogger<FornecedorRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }

        public async Task<IEnumerable<Fornecedor>> SelectFornecedores(FiltroPesquisaFornecedor filtro)
        {
            IEnumerable<Fornecedor> fornecedor = null;
            try
            {
                string sql = "pkg_bs_consultas.CONSULTAR_FORNECEDORES";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parms.Add("pNOMEFORNEC", filtro.NomeFornecedor);
                    parms.Add("pCNPJ", filtro.Cnpj);
                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    fornecedor = await conn.QueryAsync<Fornecedor>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return fornecedor;
        }
    }
}