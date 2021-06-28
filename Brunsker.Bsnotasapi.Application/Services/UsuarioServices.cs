using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Brunsker.Bsnotas.Domain.Adapters;
using Brunsker.Bsnotasapi.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Brunsker.Bsnotasapi.Application.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly IOracleRepositoryAdapter _rep;
        private readonly ILogger<UsuarioServices> _logger;
        private readonly IConfiguration _config;

        public UsuarioServices(IOracleRepositoryAdapter rep, ILogger<UsuarioServices> logger, IConfiguration config)
        {
            _rep = rep;
            _logger = logger;
            _config = config;
        }

        public async Task CriaUsuario(Usuario usuario)
        {
            _logger.LogInformation("Iniciou o processo de criaçao de usuario");

            await _rep.InsertUsuario(usuario);

            _logger.LogInformation("Usuario criado com sucesso!");
        }

        public string GeraToken(Usuario usuario)
        {
            try
            {
                _logger.LogInformation("Inicio geracao de token");

                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_config["Token:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, usuario.LOGIN.ToString())
                    }
                    ),
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                _logger.LogInformation("Token gerado com sucesso!");

                return tokenHandler.WriteToken(token);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return null;
            }
        }

        public async Task<Usuario> Login(string login, string senha)
        {
            _logger.LogInformation("Realizando Login: " + login);

            var usuario = await _rep.Login(login, senha);

            return usuario;
        }

        public async Task<Usuario> SelectUsuarioPorEmail(string email)
        {
            _logger.LogInformation("Buscando usuario por email: " + email);

            var usuario = await _rep.SelectUsuarioPorEmail(email);

            return usuario;
        }
    }
}