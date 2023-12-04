namespace DevagramCShrap.Dtos
{
    public class PublicacaoRequisicaoDto
    {
        public string Descricao { get; set; }
        public DateTime DataPublicacao { get; set; }
        public IFormFile Foto { get; set; }
    }
}
