using DevagramCShrap.Models;

namespace DevagramCShrap.Repository
{
    public interface IComentarioRepository
    {
        public void Comentar(Comentario comentario);
        List<Comentario> GetComentarioPorPublicacao(int idPublicacao);
    }
}
