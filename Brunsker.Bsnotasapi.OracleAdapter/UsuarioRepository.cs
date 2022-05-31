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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pNOME", usuario.NOME);
                    parametros.Add("pLOGIN", usuario.LOGIN);
                    parametros.Add("pSENHA", usuario.SENHA);
                    parametros.Add("pSEQ_CLIENTE", usuario.SEQ_CLIENTE);
                    parametros.Add("pAVATAR", usuario.AVATAR);

                    await conexao.ExecuteAsync("pkg_clientes_nfe.INSERT_USUARIO",
                        parametros, commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    usuario = await conexao.QueryFirstOrDefaultAsync<Usuario>
                        ($"SELECT * FROM BSNT_USERS U WHERE U.LOGIN = '{email}'");
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = new OracleDynamicParameters();

                    parametros.Add("pLOGIN", login);
                    parametros.Add("pSENHA", senha);
                    parametros.Add("CUR_OUT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                    usuario = await conexao.QuerySingleOrDefaultAsync<Usuario>
                        ("pkg_clientes_nfe.LOGIN_USUARIO", parametros, 
                        commandType: CommandType.StoredProcedure);
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
                using (var conexao = new OracleConnection(_connectionString))
                {
                    var parametros = await conexao.QueryFirstOrDefaultAsync<ParametrosCliente>
                        ($"SELECT * FROM BSNT_PARAMETROS P WHERE P.SEQ_CLIENTE = {id}");

                    return parametros;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return null;
            }
        }
        public async Task<IEnumerable<Usuario>> SelectUsuarios(long seqCliente)
        {
            IEnumerable<Usuario> usuarios = null;

            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    usuarios = await conexao.QueryAsync<Usuario>($"SELECT * FROM BSNT_USERS T WHERE T.SEQ_CLIENTE =  {seqCliente}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return usuarios;
        }
        public async Task<IEnumerable<Certificado>> SelectValidadeCertificados(long seqCliente)
        {
            IEnumerable<Certificado> certificados = null;

            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    certificados = await conexao.QueryAsync<Certificado>(
                        $@"SELECT DISTINCT (C.VALIDADE_CERTIFICADO) AS VALIDADE,
                        C.CERTIFICADO_DIGITAL AS NOMECERTIFICADO,
                        C.SEQ_CLIENTE SEQCLIENTE
                        FROM BSNT_CERTIFICADO_DIGITAL C
                        WHERE C.SEQ_CLIENTE = {seqCliente}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return certificados;
        }

        public async Task RemoveUsuario(long seqUsuario)
        {
            try
            {
                using (var conexao = new OracleConnection(_connectionString))
                {
                    await conexao.ExecuteAsync($"DELETE FROM BSNT_USERS U WHERE U.SEQ_USUARIOS = {seqUsuario}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
        }
    }
}