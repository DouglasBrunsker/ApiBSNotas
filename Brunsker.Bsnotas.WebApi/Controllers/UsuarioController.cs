using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brunsker.Bsnotas.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _services;

        public UsuarioController(IUsuarioServices services)
        {
            _services = services;
        }

        [HttpGet("Current")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioCorrente()
        {
            var login = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;

            var usuario = await _services.SelectUsuarioPorEmail(login);

            return new UsuarioDto
            {
                LOGIN = usuario.LOGIN,
                TOKEN = _services.GeraToken(usuario),
                SEQ_CLIENTE = usuario.SEQ_CLIENTE,
                NOME = usuario.NOME,
                AVATAR = usuario.AVATAR
            };
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuarioDto>> RegistrarUsuario(Usuario usuario)
        {
            var user = await _services.SelectUsuarioPorEmail(usuario.LOGIN);

            if (user != null) return BadRequest("Endereco de email em uso.");

            await _services.CriaUsuario(usuario);

            return new UsuarioDto
            {
                LOGIN = usuario.LOGIN,
                TOKEN = _services.GeraToken(usuario),
                SEQ_CLIENTE = usuario.SEQ_CLIENTE,
                NOME = usuario.NOME,
                AVATAR = usuario.AVATAR
            };
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioDto>> Login(UsuarioParaLogin loginDto)
        {
            var usuario = await _services.Login(loginDto.Login, loginDto.Senha);

            if (usuario == null) return Unauthorized();

            return new UsuarioDto
            {
                LOGIN = usuario.LOGIN,
                TOKEN = _services.GeraToken(usuario),
                SEQ_CLIENTE = usuario.SEQ_CLIENTE,
                NOME = usuario.NOME,
                AVATAR = usuario.AVATAR
            };
        }
    }
}