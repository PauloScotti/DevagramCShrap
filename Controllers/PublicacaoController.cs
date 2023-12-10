using DevagramCShrap.Dtos;
using DevagramCShrap.Models;
using DevagramCShrap.Repository;
using DevagramCShrap.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCShrap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicacaoController : BaseController
    {
        private readonly ILogger<PublicacaoController> _logger;
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly ICurtidaRepository _curtidaRepository;
        private readonly IFavoritarRepository _favoritarRepository;

        public PublicacaoController(ILogger<PublicacaoController> logger,
            IPublicacaoRepository publicacaoRepository,
            IUsuarioRepository usuarioRepository,
            IComentarioRepository comentarioRepository,
            IFavoritarRepository favoritarRepository,
            ICurtidaRepository curtidaRepository
            ) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
            _comentarioRepository = comentarioRepository;
            _curtidaRepository = curtidaRepository;
            _favoritarRepository = favoritarRepository;
        }

        [HttpPost]
        public IActionResult Publicar([FromForm] PublicacaoRequisicaoDto publicacaoDto)
        {
            try
            {
                Usuario usuario = LerToken();
                CosmicService cosmicService = new CosmicService();
                if(publicacaoDto != null) {
                    if(String.IsNullOrEmpty(publicacaoDto.Descricao)
                        && String.IsNullOrWhiteSpace(publicacaoDto.Descricao))
                    {
                        _logger.LogError("A descrição está inválida");
                        return BadRequest("É obrigatório incluir a descrição na publicação");
                    }
                    if(publicacaoDto.Foto == null)
                    {
                        _logger.LogError("A foto está inválida");
                        return BadRequest("É obrigatório incluir a foto na publicação");
                    }
                    string contentType = publicacaoDto.Foto.ContentType;
                    Publicacao publicacao = new Publicacao()
                    {
                        Descricao = publicacaoDto.Descricao,
                        IdUsuario = usuario.Id,
                        DataPublicacao = DateTime.Now,
                        FileType = publicacaoDto.Foto.ContentType,
                        Foto = cosmicService.EnviarImagem(new ImagemDto { Imagem = publicacaoDto.Foto, Nome = publicacaoDto.Foto.FileName })
                    };
                    _publicacaoRepository.Publicar(publicacao);
                }

                return Ok("Publicação salva com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao criar publicação");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("feed")]
        public IActionResult FeedHome()
        {
            try
            {
                List<PublicacaoFeedRespostaDto> feed = _publicacaoRepository.GetPublicacoesFeed(LerToken().Id);
                
                foreach (PublicacaoFeedRespostaDto feedRespostaDto in feed)
                {
                    Usuario usuario = _usuarioRepository.GetUsuarioPorId(feedRespostaDto.IdUsuario);
                    UsuarioRespostaDto usuarioRespostaDto = new UsuarioRespostaDto()
                    {
                        Nome = usuario.Nome,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id
                    };
                    feedRespostaDto.Usuario = usuarioRespostaDto;

                    List<Comentario> comentarios = _comentarioRepository.GetComentarioPorPublicacao(feedRespostaDto.IdPublicacao);
                    feedRespostaDto.Comentarios = comentarios;

                    List<Curtida> curtidas = _curtidaRepository.GetCurtidaPorPublicacao(feedRespostaDto.IdPublicacao);
                    feedRespostaDto.Curtidas = curtidas;

                }
                
                return Ok(feed);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao carregar o feed da home");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("feedusuario")]
        public IActionResult FeedUsuario(int idUsuario)
        {
            try
            {
                List<PublicacaoFeedRespostaDto> feed = _publicacaoRepository.GetPublicacoesFeedUsuario(idUsuario);

                foreach (PublicacaoFeedRespostaDto feedRespostaDto in feed)
                {
                    Usuario usuario = _usuarioRepository.GetUsuarioPorId(feedRespostaDto.IdUsuario);
                    UsuarioRespostaDto usuarioRespostaDto = new UsuarioRespostaDto()
                    {
                        Nome = usuario.Nome,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id
                    };
                    feedRespostaDto.Usuario = usuarioRespostaDto;

                    List<Comentario> comentarios = _comentarioRepository.GetComentarioPorPublicacao(feedRespostaDto.IdPublicacao);
                    feedRespostaDto.Comentarios = comentarios;

                    List<Curtida> curtidas = _curtidaRepository.GetCurtidaPorPublicacao(feedRespostaDto.IdPublicacao);
                    feedRespostaDto.Curtidas = curtidas;

                }

                return Ok(feed);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao carregar o feed do usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("publicacaoId")]
        public IActionResult PublicacaoId(int idPublicacao)
        {
            try
            {
                return Ok(_publicacaoRepository.GetPublicacaoId(idPublicacao));
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao buscar a publicação");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("favoritas")]
        public IActionResult PesquisarFavoritas()
        {
            try
            {
                List<Favoritar> favoritas = _favoritarRepository.GetFavoritas(LerToken().Id);

                List<PublicacaoFeedRespostaDto> publicacoes = new List<PublicacaoFeedRespostaDto>();

                foreach (Favoritar favoritar in favoritas)
                {
                    Publicacao publicacao = _publicacaoRepository.GetPublicacaoId(favoritar.IdPublicacao);
                    Usuario usuario = _usuarioRepository.GetUsuarioPorId(favoritar.IdUsuario);
                    UsuarioRespostaDto usuarioRespostaDto = new UsuarioRespostaDto()
                    {
                        Nome = usuario.Nome,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id
                    };

                    PublicacaoFeedRespostaDto publicacaoFeedRespostaDto = new PublicacaoFeedRespostaDto()
                    {
                        Usuario = usuarioRespostaDto,
                        Descricao = publicacao.Descricao,
                        Foto = publicacao.Foto,
                        DataPublicacao = publicacao.DataPublicacao,
                        FileType = publicacao.FileType,
                        IdPublicacao = publicacao.Id,
                        IdUsuario = publicacao.IdUsuario
                    };

                    List<Comentario> comentarios = _comentarioRepository.GetComentarioPorPublicacao(publicacaoFeedRespostaDto.IdPublicacao);
                    publicacaoFeedRespostaDto.Comentarios = comentarios;

                    List<Curtida> curtidas = _curtidaRepository.GetCurtidaPorPublicacao(publicacaoFeedRespostaDto.IdPublicacao);
                    publicacaoFeedRespostaDto.Curtidas = curtidas;

                    publicacoes.Add(publicacaoFeedRespostaDto);

                }
                return Ok(publicacoes);
                }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao carregar o feed da home");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost]
        [Route("compartilhar")]
        public IActionResult CompartilharPublicacao(int idPublicacao)
        {
            try
            {
                Usuario usuario = LerToken();
                var publicacaoASerCompartilhada = _publicacaoRepository.getQtdePublicacaoId(idPublicacao);

                if (publicacaoASerCompartilhada != null)
                {
                    if (String.IsNullOrEmpty(publicacaoASerCompartilhada.Descricao)
                        && String.IsNullOrWhiteSpace(publicacaoASerCompartilhada.Descricao))
                    {
                        _logger.LogError("A descrição está inválida");
                        return BadRequest("É obrigatório incluir a descrição na publicação");
                    }
                    if (publicacaoASerCompartilhada.Foto == null)
                    {
                        _logger.LogError("A foto está inválida");
                        return BadRequest("É obrigatório incluir a foto na publicação");
                    }
                    Publicacao publicacao = new Publicacao()
                    {
                        Descricao = publicacaoASerCompartilhada.Descricao,
                        IdUsuario = usuario.Id,
                        DataPublicacao = DateTime.Now,
                        FileType = publicacaoASerCompartilhada.FileType,
                        Foto = publicacaoASerCompartilhada.Foto
                    };
                    _publicacaoRepository.Publicar(publicacao);
                }

                return Ok("Publicação compartilhada com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao criar publicação");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
