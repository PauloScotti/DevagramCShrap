﻿using DevagramCShrap.Dtos;
using DevagramCShrap.Models;
using DevagramCShrap.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCShrap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguidorController : BaseController
    {
        private readonly ILogger<SeguidorController> _logger;
        private readonly ISeguidorRepository _seguidorRepository;

        public SeguidorController(ILogger<SeguidorController> logger,
            ISeguidorRepository seguidorRepository,
            IUsuarioRepository usuarioRepository
            ) : base(usuarioRepository)
        {
            _logger = logger;
            _seguidorRepository = seguidorRepository;
        }

        [HttpPut]
        public IActionResult Seguir(int idSeguido)
        {
            try
            {
                Usuario usuarioseguido = _usuarioRepository.GetUsuarioPorId(idSeguido);
                Usuario usuarioseguidor = LerToken();


                if(usuarioseguido != null)
                {
                    Seguidor seguidor = _seguidorRepository.GetSeguidor(usuarioseguidor.Id, usuarioseguido.Id);
                    if(seguidor != null)
                    {
                        _seguidorRepository.Desseguir(seguidor);
                        return Ok("Desseguir realizado com sucesso!");
                    }
                    else
                    {
                        Seguidor seguidornovo = new Seguidor()
                        {
                            IdUsuarioSeguido = usuarioseguido.Id,
                            IdUsuarioSeguidor = usuarioseguidor.Id
                        };
                        _seguidorRepository.Seguir(seguidornovo);
                        return Ok("Seguir realizado com sucesso!");
                    }
                }
                else
                {
                    return BadRequest("Ocorreu um erro ao seguir/desseguir");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao seguir o usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
                {
                    Descricao = "Ocorreu o seguinte erro: " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
