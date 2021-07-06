using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Brunsker.Bsnotasapi.Application.Services
{
    public class UsuarioServices : IUsuarioServices
    {

        private readonly ILogger<UsuarioServices> _logger;
        private readonly IConfiguration _config;

        public UsuarioServices(ILogger<UsuarioServices> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
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
    }
}