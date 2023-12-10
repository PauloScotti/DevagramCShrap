using System.ComponentModel.DataAnnotations.Schema;

namespace DevagramCShrap.Models
{
    public class Publicacao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string? FileType { get; set; }
        public string Foto { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataPublicacao { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; private set; }

    }
}
