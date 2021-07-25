using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioServices services, IUsuarioRepository rep, IMapper mapper)
        {
            _services = services;
            _rep = rep;
            _mapper = mapper;
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

        [HttpGet("parametros/{id}")]
        public async Task<IActionResult> GetParametros(long id)
        {
            var parametros = await _rep.SelectParametros(id);

            return Ok(parametros);
        }

        [HttpGet("BucarUsuarios/{seqCliente}")]
        public async Task<IActionResult> BuscarUsuario(long seqCliente)
        {
            var usuarios = _mapper.Map<IEnumerable<UsuarioDto>>(await _rep.SelectUsuarios(seqCliente));

            return Ok(usuarios.OrderBy(u => u.SEQ_USUARIOS));
        }

        [HttpGet("BucarValidadeCertificados/{seqCliente}")]
        public async Task<IActionResult> BucarValidadeCertificados(long seqCliente)
        {
            var certificados = await _rep.SelectValidadeCertificados(seqCliente);

            return Ok(certificados);
        }

        [HttpDelete("{seqUsuario}")]
        public async Task<IActionResult> Remover(long seqUsuario)
        {
            await _rep.RemoveUsuario(seqUsuario);

            return Ok();
        }
    }
}