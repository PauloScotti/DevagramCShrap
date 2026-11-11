using DevagramCShrap.Dtos;
using DevagramCShrap.Models;
using DevagramCShrap.Repository;
using DevagramCShrap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCShrap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;

        public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository, TokenService tokenService)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult EfetuarLogin([FromBody] LoginRequisicaoDto loginrequisicao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginrequisicao?.Email) || string.IsNullOrWhiteSpace(loginrequisicao.Senha))
                {
                    return BadRequest(new ErrorRespostaDto()
                    {
                        Descricao = "Usuário não digitou corretamente os campos de login corretamente",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                // Mesma resposta para e-mail inexistente e senha errada: não revela
                // quais e-mails têm conta.
                Usuario? usuario = _usuarioRepository.GetUsuarioPorEmail(loginrequisicao.Email.ToLower());
                var (valida, precisaRegravar) = usuario == null
                    ? (false, false)
                    : SenhaService.Conferir(loginrequisicao.Senha, usuario.Senha);
                if (usuario == null || !valida)
                {
                    return BadRequest(new ErrorRespostaDto()
                    {
                        Descricao = "Email ou senha inválidos",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                if (precisaRegravar)
                {
                    usuario.Senha = SenhaService.GerarHash(loginrequisicao.Senha);
                    _usuarioRepository.AtualizarUsuario(usuario);
                }

                return Ok(new LoginRespostaDto()
                {
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    Token = _tokenService.CriarToken(usuario)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro no login");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu um erro no login",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
