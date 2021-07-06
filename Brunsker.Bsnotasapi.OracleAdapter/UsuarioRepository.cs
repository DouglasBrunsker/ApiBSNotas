using System;
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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuarioRepository> _logger;
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration, ILogger<UsuarioRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }

        public async Task InsertUsuario(Usuario usuario)
        {
            try
            {
                string sql = "pkg_clientes_nfe.INSERT_USUARIO";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pNOME", usuario.NOME);
                    parms.Add("pLOGIN", usuario.LOGIN);
                    parms.Add("pSENHA", usuario.SENHA);
                    parms.Add("pSEQ_CLIENTE", usuario.SEQ_CLIENTE);
                    parms.Add("pAVATAR", usuario.AVATAR);

                    await conn.ExecuteAsync(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
        }
        public async Task<Usuario> SelectUsuarioPorEmail(string email)
        {
            Usuario usuario = null;
            try
            {
                string sql = $"SELECT * FROM BSNT_USERS U WHERE U.LOGIN = '{email}'";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    usuario = await conn.QueryFirstOrDefaultAsync<Usuario>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return usuario;
        }
        public async Task<Usuario> Login(string login, string senha)
        {
            Usuario usuario = null;

            try
            {
                string sql = "pkg_clientes_nfe.LOGIN_USUARIO";

                using (var conn = new OracleConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var parms = new OracleDynamicParameters();

                    parms.Add("pLOGIN", login);
                    parms.Add("pSENHA", senha);
                    parms.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    usuario = await conn.QuerySingleOrDefaultAsync<Usuario>(sql, parms, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return usuario;
        }
        public async Task<ParametrosCliente> SelectParametros(long id)
        {
            try
            {
                string sql = $"SELECT * FROM BSNT_PARAMETROS P WHERE P.SEQ_CLIENTE = {id}";

                using (var conn = new OracleConnection(_connectionString))
                {
                    var parametros = await conn.QueryFirstOrDefaultAsync<ParametrosCliente>(sql);

                    return parametros;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return null;
            }
        }
    }
}