using DevagramCShrap.Dtos;
using DevagramCShrap.Models;

namespace DevagramCShrap.Repository
{
    public interface IPublicacaoRepository
    {
        List<PublicacaoFeedRespostaDto> GetPublicacoesFeed(int idUsuario);
        List<PublicacaoFeedRespostaDto> GetPublicacoesFeedUsuario(int idUsuario);
        public void Publicar(Publicacao publicacao);
    }
}
