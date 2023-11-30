using System.Security.Claims;
using DevagramCShrap.Models;
using DevagramCShrap.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCShrap.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly IUsuarioRepository _usuarioRepository;

        public BaseController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        protected Usuario LerToken()
        {
            var idUsuario = User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).FirstOrDefault();

            if(string.IsNullOrEmpty(idUsuario) )
            {
                return null;
            }
            else
            {
                return _usuarioRepository.GetUsuarioPorId(int.Parse(idUsuario));
            }
        }
    }
}
