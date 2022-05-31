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
                using (var conexao = new OracleConnection(_connectionString))
                {             
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parametros.Add("pNOMEFORNEC", filtro.NomeFornecedor);
                    parametros.Add("pCNPJ", filtro.Cnpj);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor,
                        direction: ParameterDirection.Output);

                    fornecedor = await conexao.QueryAsync<Fornecedor>
                        ("pkg_bs_consultas.CONSULTAR_FORNECEDORES",
                        parametros, commandType: CommandType.StoredProcedure);
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