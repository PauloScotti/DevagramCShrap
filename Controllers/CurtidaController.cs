using DevagramCShrap.Dtos;
using DevagramCShrap.Models;
using DevagramCShrap.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCShrap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurtidaController : BaseController
    {
        private readonly ILogger<CurtidaController> _logger;
        private readonly ICurtidaRepository _curtidaRepository;

        public CurtidaController(ILogger<CurtidaController> logger,
            ICurtidaRepository curtidaRepository,
            IUsuarioRepository usuarioRepository
            ) : base(usuarioRepository)
        {
            _logger = logger;
            _curtidaRepository = curtidaRepository;
        }

        [HttpPut]
        public IActionResult Curtir([FromBody] CurtidaRequisicaoDto curtidaDto)
        {
            try
            {
                if (curtidaDto != null)
                {
                    Curtida curtida = _curtidaRepository.GetCurtida(curtidaDto.IdPublicacao, LerToken().Id);

                    if(curtida != null)
                    {
                        _curtidaRepository.Descurtir(curtida);
                        return Ok("Descurtida realizada com suceso!");
                    }
                    else
                    {
                        Curtida curtidanova = new Curtida()
                        {
                            IdPublicacao = curtidaDto.IdPublicacao,
                            IdUsuario = LerToken().Id
                        };

                        _curtidaRepository.Curtir(curtidanova);
                        return Ok("Curtida realizada com suceso!");
                    }
                    
                }
                else
                {
                    _logger.LogError("A requisição de curtir está vazia");
                    return BadRequest("A requisição de curtir está vazia");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao curtir/descurtir a publicação");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
