using DevagramCShrap.Models;

namespace DevagramCShrap.Repository
{
    public interface IUsuarioRepository
    {
        Usuario GetUsuarioPorLoginSenha(string v1, string v2);
        public void Salvar(Usuario usuario);

        public bool VerificarEmail(string email);

        Usuario GetUsuarioPorId(int id);

        public void AtualizarUsuario (Usuario usuario);
        List<Usuario> GetUsuarioPorNome(string nome);
    }
}
