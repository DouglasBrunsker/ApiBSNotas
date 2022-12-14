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
                string sql = "pkg_bs_consultas.CONSULTAR_PRODUTOS";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pSEQ_CLIENTE", filtro.SeqCliente);
                    parms.Add("pNOMEFORNEC", filtro.NomeFornecedor);
                    parms.Add("pNOMEPRODUTO", filtro.NomeProduto);
                    parms.Add("pCODPRODUTO", filtro.CodeProduto);
                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);


                    produtos = await conn.QueryAsync<Produto>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return produtos;
        }

        public async Task<ICMS> ExibirICMS(string chave, string CODPROD)
        {
            ICMS ICMS = null;

            try
            {
                using (var coneccao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pCHAVE", chave);
                    parametros.Add("pCODPROD", CODPROD);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    ICMS = await coneccao.QueryFirstOrDefaultAsync<ICMS>("PKG_PRE_ENTRADA.PROC_EXIBIR_ICMS", parametros, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }

            return ICMS;
        }

        public async Task<IEnumerable<CodProd>> SearchCPROD(string chave)
        {
            IEnumerable<CodProd> fornecedores = null;

            try
            {
                string query = $"SELECT CPROD FROM BSNOTASNOVO.BSNT_PRODUTO_NFE  WHERE CHAVE = '{chave}'";

                using (var conn = new OracleConnection(_connectionString))
                {

                    fornecedores = await conn.QueryAsync<CodProd>(query);
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