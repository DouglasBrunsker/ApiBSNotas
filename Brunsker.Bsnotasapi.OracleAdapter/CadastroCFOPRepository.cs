using Brunsker.Bsnotas.Domain.Interfaces;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.OracleAdapter;
using Dapper;
using Dapper.Oracle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.OracleAdapter
{
    public class CadastroCFOPRepository : ICadastroCFOPRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CadastroCFOPRepository> _logger;
        private readonly string _connectionString;

        public CadastroCFOPRepository(IConfiguration configuration, ILogger<CadastroCFOPRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }

        public async Task CadastrarCFOP(int seqCliente, string stringBanco, int cfopEnd, int cfoSaida, string descricao)
        {
             CadastroCFOP cadastroCFOP = null;

            try
            {
                using (var coneccao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pSEQ_CLIENTE", seqCliente);
                    parametros.Add("pSTRING_BANCO ", stringBanco);
                    parametros.Add("pCFOPENT", cfopEnd);
                    parametros.Add("pCFOPSAIDA", cfoSaida);
                    parametros.Add("pDESCRICAO", descricao);
                    //  parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    cadastroCFOP = await coneccao.QueryFirstOrDefaultAsync<CadastroCFOP>("PKG_WEBSERV_INSERT_BSNOTAS.PROC_REGRACFOPXMLPED", parametros, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }        
        }
    }
}
