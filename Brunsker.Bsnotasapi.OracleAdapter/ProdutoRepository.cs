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
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProdutoRepository> _logger;
        private readonly string _connectionString;

        public ProdutoRepository(IConfiguration configuration, ILogger<ProdutoRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }

        public async Task<IEnumerable<Produto>> SelectProdutos(FiltroPesquisaProdutos filtro)
        {
            IEnumerable<Produto> produtos = null;
            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parametros.Add("pNOMEFORNEC", filtro.NomeFornecedor);
                    parametros.Add("pNOMEPRODUTO", filtro.NomeProduto);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    produtos = await conexao.QueryAsync<Produto>("pkg_bs_consultas.CONSULTAR_PRODUTOS", parametros, 
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return produtos;
        }
    }
}