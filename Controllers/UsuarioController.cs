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
    public class UsuarioController : BaseController
    {
        private const int SenhaMinima = 8;

        private readonly ILogger<UsuarioController> _logger;
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly ISeguidorRepository _seguidorRepository;
        private readonly CosmicService _cosmicService;

        public UsuarioController(ILogger<UsuarioController> logger,
            IUsuarioRepository usuarioRepository, IPublicacaoRepository publicacaoRepository,
            ISeguidorRepository seguidorRepository, CosmicService cosmicService) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
            _seguidorRepository = seguidorRepository;
            _cosmicService = cosmicService;
        }

        [HttpGet]
        public IActionResult ObterUsuario()
        {
            try
            {
                Usuario usuario = LerToken();
                return Ok(new UsuarioRespostaDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                });
            }
            catch (Exception ex)
            {
                return ErroInterno(ex, "Ocorreu um erro ao obter o usuário");
            }
        }

        [HttpPut]
        public IActionResult AtualizarUsuario([FromForm] UsuarioRequisicaoDto usuariodto)
        {
            try
            {
                Usuario usuario = LerToken();
                if (string.IsNullOrWhiteSpace(usuariodto?.Nome))
                {
                    return DadosInvalidos(new List<string> { "Nome inválido" });
                }

                if (usuariodto.FotoPerfil != null)
                {
                    usuario.FotoPerfil = EnviarFoto(usuariodto);
                }
                usuario.Nome = usuariodto.Nome;
                _usuarioRepository.AtualizarUsuario(usuario);

                return Ok("Usuário foi salvo com sucesso");
            }
            catch (ImagemInvalidaException ex)
            {
                return DadosInvalidos(new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return ErroInterno(ex, "Ocorreu um erro ao salvar o usuário");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SalvarUsuario([FromForm] UsuarioRequisicaoDto usuariodto)
        {
            try
            {
                var erros = new List<string>();
                if (string.IsNullOrWhiteSpace(usuariodto?.Nome))
                {
                    erros.Add("Nome inválido");
                }
                if (string.IsNullOrWhiteSpace(usuariodto?.Email) || !usuariodto.Email.Contains('@'))
                {
                    erros.Add("E-mail inválido");
                }
                if (string.IsNullOrWhiteSpace(usuariodto?.Senha) || usuariodto.Senha.Length < SenhaMinima)
                {
                    erros.Add($"A senha precisa ter pelo menos {SenhaMinima} caracteres");
                }
                if (erros.Count > 0)
                {
                    return DadosInvalidos(erros);
                }

                // Confere o e-mail antes de enviar a imagem: cadastro repetido não gasta upload.
                var email = usuariodto!.Email!.ToLower();
                if (_usuarioRepository.VerificarEmail(email))
                {
                    return BadRequest(new ErrorRespostaDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Descricao = "Usuário já está cadastrado!"
                    });
                }

                _usuarioRepository.Salvar(new Usuario()
                {
                    Email = email,
                    Senha = SenhaService.GerarHash(usuariodto.Senha!),
                    Nome = usuariodto.Nome,
                    FotoPerfil = usuariodto.FotoPerfil == null ? "" : EnviarFoto(usuariodto)
                });

                return Ok("Usuário foi salvo com sucesso");
            }
            catch (ImagemInvalidaException ex)
            {
                return DadosInvalidos(new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return ErroInterno(ex, "Ocorreu um erro ao salvar o usuário");
            }
        }

        [HttpGet]
        [Route("pesquisaid")]
        public IActionResult PesquisarUsuarioId(int idUsuario)
        {
            try
            {
                Usuario usuario = _usuarioRepository.GetUsuarioPorId(idUsuario);
                if (usuario == null)
                {
                    return NotFound(new ErrorRespostaDto()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Descricao = "Usuário não encontrado"
                    });
                }
                return Ok(ParaResposta(usuario));
            }
            catch (Exception ex)
            {
                return ErroInterno(ex, "Ocorreu um erro ao pesquisar o usuário");
            }
        }

        [HttpGet]
        [Route("pesquisanome")]
        public IActionResult PesquisarUsuarioNome(string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome) || nome.Trim().Length < 2)
                {
                    return DadosInvalidos(new List<string> { "Informe pelo menos 2 caracteres para a busca" });
                }
                List<Usuario> usuarios = _usuarioRepository.GetUsuarioPorNome(nome.Trim());
                return Ok(usuarios.Select(ParaResposta).ToList());
            }
            catch (Exception ex)
            {
                return ErroInterno(ex, "Ocorreu um erro ao pesquisar o usuário");
            }
        }

        private string EnviarFoto(UsuarioRequisicaoDto usuariodto)
        {
            return _cosmicService.EnviarImagem(new ImagemDto
            {
                Imagem = usuariodto.FotoPerfil!,
                Nome = usuariodto.Nome.Replace(" ", "")
            });
        }

        private UsuarioRespostaDto ParaResposta(Usuario usuario)
        {
            return new UsuarioRespostaDto
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Avatar = usuario.FotoPerfil,
                IdUsuario = usuario.Id,
                QtdePublicacoes = _publicacaoRepository.getQtdePublicacoes(usuario.Id),
                QtdeSeguidores = _seguidorRepository.getQtdeSeguidores(usuario.Id),
                QtdeSeguindo = _seguidorRepository.getQtdeSeguindo(usuario.Id)
            };
        }

        private IActionResult DadosInvalidos(List<string> erros)
        {
            return BadRequest(new ErrorRespostaDto()
            {
                Status = StatusCodes.Status400BadRequest,
                Erros = erros
            });
        }

        // O detalhe da exceção fica no log; o cliente recebe só uma mensagem genérica.
        private IActionResult ErroInterno(Exception ex, string contexto)
        {
            _logger.LogError(ex, "{Contexto}", contexto);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
            {
                Descricao = "Ocorreu um erro interno",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
}
