using DevagramCShrap.Dtos;
using DevagramCShrap.Models;
using DevagramCShrap.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCShrap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : BaseController
    {
        private readonly ILogger<ComentarioController> _logger;
        private readonly IComentarioRepository _comentarioRepository;

        public ComentarioController (ILogger<ComentarioController> logger,
            IComentarioRepository comentarioRepository, IUsuarioRepository usuarioRepository) : base (usuarioRepository)
        {
            _logger = logger;
            _comentarioRepository = comentarioRepository;
        }

        [HttpPut]
        public IActionResult Comentar([FromBody] ComentarioRequisicaoDto comentarioDto)
        {
            try
            {
                if(comentarioDto != null)
                {
                    if(String.IsNullOrEmpty(comentarioDto.Descricao) || String.IsNullOrWhiteSpace(comentarioDto.Descricao))
                    {
                        _logger.LogError("O comentário está inválido");
                        return BadRequest("Por favor coloque seu comentário");
                    }

                    Comentario comentario = new Comentario();
                    comentario.Descricacao = comentarioDto.Descricao;
                    comentario.IdPublicacao = comentarioDto.IdPublicacao;
                    comentario.IdUsuario = LerToken().Id;

                    _comentarioRepository.Comentar(comentario);

                }
                return Ok("Comentário enviado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao comentar");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
