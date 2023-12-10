using DevagramCShrap.Models;

namespace DevagramCShrap.Dtos
{
    public class FavoritarRespostaDto
    {
        public int IdPublicacao { get; set; }
        public string Descricao { get; set; }
        public string Foto { get; set; }
        public string FileType { get; set; }
        public DateTime DataPublicacao { get; set; }
        public int IdUsuario { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<Curtida> Curtidas { get; set; }
        public UsuarioRespostaDto Usuario { get; set; }
        public PublicacaoFeedRespostaDto Publicacao {  get; set; }
    }
}
