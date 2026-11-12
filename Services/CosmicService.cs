using System.Net.Http.Headers;
using DevagramCShrap.Dtos;

namespace DevagramCShrap.Services
{
    public class ImagemInvalidaException : Exception
    {
        public ImagemInvalidaException(string mensagem) : base(mensagem) { }
    }

    public class CosmicService
    {
        private const long TamanhoMaximo = 5 * 1024 * 1024;
        private static readonly string[] TiposAceitos = { "image/jpeg", "image/png", "image/webp", "image/gif" };

        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public CosmicService(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }

        public string EnviarImagem(ImagemDto imagemdto)
        {
            var imagem = imagemdto.Imagem;
            if (imagem == null || imagem.Length == 0)
            {
                throw new ImagemInvalidaException("A imagem é obrigatória");
            }
            if (!TiposAceitos.Contains(imagem.ContentType))
            {
                throw new ImagemInvalidaException("A imagem precisa ser JPEG, PNG, WebP ou GIF");
            }
            if (imagem.Length > TamanhoMaximo)
            {
                throw new ImagemInvalidaException("A imagem pode ter no máximo 5 MB");
            }

            // Bucket e token vêm da configuração (Cosmic__Bucket e Cosmic__Token), nunca do código.
            var bucket = _configuration["Cosmic:Bucket"];
            var token = _configuration["Cosmic:Token"];
            if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("Defina Cosmic:Bucket e Cosmic:Token para enviar imagens.");
            }

            using var conteudo = new MultipartFormDataContent
            {
                { new StreamContent(imagem.OpenReadStream()), "media", imagemdto.Nome },
            };
            using var requisicao = new HttpRequestMessage(HttpMethod.Post, $"https://workers.cosmicjs.com/v3/buckets/{Uri.EscapeDataString(bucket)}/media")
            {
                Content = conteudo,
            };
            requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var resposta = _http.Send(requisicao);
            resposta.EnsureSuccessStatusCode();
            var corpo = resposta.Content.ReadFromJsonAsync<CosmicRespostaDto>().GetAwaiter().GetResult();
            return corpo?.media?.url ?? throw new InvalidOperationException("O Cosmic não devolveu a URL da imagem.");
        }
    }
}
