using DevagramCShrap.Dtos;
using DevagramCShrap.Models;

namespace DevagramCShrap.Repository
{
    public interface IPublicacaoRepository
    {
        List<PublicacaoFeedRespostaDto> GetPublicacoesFeed(int idUsuario);
        List<PublicacaoFeedRespostaDto> GetPublicacoesFeedUsuario(int idUsuario);
        int getQtdePublicacoes(int idUsuario);
        Publicacao getQtdePublicacaoId(int idPublicacao);
        public void Publicar(Publicacao publicacao);
        Publicacao GetPublicacaoId(int idPublicacao);
    }
}
