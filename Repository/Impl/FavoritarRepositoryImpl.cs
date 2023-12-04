using DevagramCShrap.Models;

namespace DevagramCShrap.Repository.Impl
{
    public class FavoritarRepositoryImpl : IFavoritarRepository
    {
        private readonly DevagramContext _context;

        public FavoritarRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }
        public void Favoritar(Favoritar favoritar)
        {
            _context.Add(favoritar);
            _context.SaveChanges();
        }

        public void Desfavoritar(Favoritar favoritar)
        {
            _context.Remove(favoritar);
            _context.SaveChanges();
        }
        public Favoritar GetFavorita(int idPubicacao, int idUsuario)
        {
            return _context.Favoritar.FirstOrDefault(f => f.IdPublicacao == idPubicacao && f.IdUsuario == idUsuario);
        }

        public List<Favoritar> GetFavoritas(int idUsuario)
        {
            return _context.Favoritar.Where(c => c.IdUsuario == idUsuario).ToList();
        }
    }
}
