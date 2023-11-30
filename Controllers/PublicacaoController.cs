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

        public PublicacaoController(ILogger<PublicacaoController> logger,
            IPublicacaoRepository publicacaoRepository,
            IUsuarioRepository usuarioRepository
            ) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
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
                    Publicacao publicacao = new Publicacao()
                    {
                        Descricao = publicacaoDto.Descricao,
                        IdUsuario = usuario.Id,
                        Foto = cosmicService.EnviarImagem(new ImagemDto { Imagem = publicacaoDto.Foto, Nome = "publicacao" })
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
    }
}
