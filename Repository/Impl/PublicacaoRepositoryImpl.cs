using DevagramCShrap.Dtos;
using DevagramCShrap.Models;

namespace DevagramCShrap.Repository.Impl
{
    public class PublicacaoRepositoryImpl : IPublicacaoRepository
    {
        private readonly DevagramContext _context;

        public PublicacaoRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }

        public Publicacao GetPublicacaoId(int idPublicacao)
        {
            return _context.Publicacoes.FirstOrDefault(p => p.Id == idPublicacao);
        }

        public List<PublicacaoFeedRespostaDto> GetPublicacoesFeed(int idUsuario)
        {
            var feed =
                from publicacoes in _context.Publicacoes
                join seguidores in _context.Seguidores on publicacoes.IdUsuario equals seguidores.IdUsuarioSeguido
                where seguidores.IdUsuarioSeguidor == idUsuario
                orderby publicacoes.DataPublicacao descending
                select new PublicacaoFeedRespostaDto
                {
                    IdPublicacao = publicacoes.Id,
                    Descricao = publicacoes.Descricao,
                    Foto = publicacoes.Foto,
                    FileType = publicacoes.FileType,
                    DataPublicacao = publicacoes.DataPublicacao,
                    IdUsuario = publicacoes.IdUsuario,
                };

            return feed.ToList();
        }

        public List<PublicacaoFeedRespostaDto> GetPublicacoesFeedUsuario(int idUsuario)
        {
            var feedusuario = 
                from publicacoes in _context.Publicacoes
                where publicacoes.IdUsuario == idUsuario
                orderby publicacoes.DataPublicacao descending
                select new PublicacaoFeedRespostaDto
                {
                    IdPublicacao = publicacoes.Id,
                    Descricao = publicacoes.Descricao,
                    Foto = publicacoes.Foto,
                    FileType= publicacoes.FileType,
                    DataPublicacao = publicacoes.DataPublicacao,
                    IdUsuario = publicacoes.IdUsuario,
                };

            return feedusuario.ToList();
        }

        public Publicacao getQtdePublicacaoId(int idPublicacao)
        {
            return _context.Publicacoes.FirstOrDefault(p => p.Id == idPublicacao);
        }

        public int getQtdePublicacoes(int idUsuario)
        {
            return _context.Publicacoes.Count(p => p.IdUsuario == idUsuario);
        }

        public void Publicar(Publicacao publicacao)
        {
            _context.Add(publicacao);
            _context.SaveChanges();
        }
    }
}
