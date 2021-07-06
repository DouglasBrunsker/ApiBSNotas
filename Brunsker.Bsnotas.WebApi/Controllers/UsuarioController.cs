using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
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
        private readonly IUsuarioRepository _rep;
        public UsuarioController(IUsuarioServices services, IUsuarioRepository rep)
        {
            _services = services;
            _rep = rep;
        }

        [HttpGet("Current")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioCorrente()
        {
            var login = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;

            var usuario = await _rep.SelectUsuarioPorEmail(login);

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
            var user = await _rep.SelectUsuarioPorEmail(usuario.LOGIN);

            if (user != null) return BadRequest("Endereco de email em uso.");

            await _rep.InsertUsuario(usuario);

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
            var usuario = await _rep.Login(loginDto.Login, loginDto.Senha);

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

        [AllowAnonymous]
        [HttpGet("parametros/{id}")]
        public async Task<IActionResult> GetParametros(long id)
        {
            return Ok(await _rep.SelectParametros(id));
        }
    }
}