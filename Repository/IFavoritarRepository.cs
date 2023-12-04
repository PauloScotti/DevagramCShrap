using DevagramCShrap.Models;

namespace DevagramCShrap.Repository
{
    public interface IFavoritarRepository
    {
        public void Favoritar(Favoritar favoritar);
        public void Desfavoritar(Favoritar favoritar);
        public List<Favoritar> GetFavoritas(int idUsuario);
        public Favoritar GetFavorita(int idPublicacao, int idUsuario);
    }
}
