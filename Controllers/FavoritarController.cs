using DevagramCShrap.Dtos;
using DevagramCShrap.Models;
using DevagramCShrap.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCShrap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritarController : BaseController
    {
        private readonly ILogger<FavoritarController> _logger;
        private readonly IFavoritarRepository _favoritarRepository;
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly ICurtidaRepository _curtidaRepository;

        public FavoritarController(ILogger<FavoritarController> logger,
            IFavoritarRepository favoritarRepository,
            IPublicacaoRepository publicacaoRepository,
            IUsuarioRepository usuarioRepository,
            IComentarioRepository comentarioRepository,
            ICurtidaRepository curtidaRepository
            ) : base(usuarioRepository)
        {
            _logger = logger;
            _favoritarRepository = favoritarRepository;
            _publicacaoRepository = publicacaoRepository;
            _comentarioRepository = comentarioRepository;
            _curtidaRepository = curtidaRepository;
        }

        [HttpPut]
        public IActionResult Favoritar([FromBody] FavoritarRequisicaoDto favoritarDto)
        {
            try
            {
                if (favoritarDto != null)
                {
                    Favoritar favorita = _favoritarRepository.GetFavorita(favoritarDto.IdPublicacao, LerToken().Id);

                    if (favorita != null)
                    {
                        _favoritarRepository.Desfavoritar(favorita);
                        return Ok("Publicação removida dos favoritos com suceso!");
                    }
                    else
                    {
                        Favoritar favoritarPublicacao = new Favoritar()
                        {
                            IdPublicacao = favoritarDto.IdPublicacao,
                            IdUsuario = LerToken().Id
                        };

                        _favoritarRepository.Favoritar(favoritarPublicacao);
                        return Ok("Publicação incluída nos favoritos com suceso!");
                    }

                }
                else
                {
                    _logger.LogError("A requisição de favoritar está vazia");
                    return BadRequest("A requisição de favoritar está vazia");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao favoritar/desfavoritar a publicação");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        public IActionResult ListarFavoritas()
        {
            try
            {
                List<Favoritar> favoritar = _favoritarRepository.GetFavoritas(LerToken().Id);
                return Ok(favoritar);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao listar publicações favoritas");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
