using System.Net.Http.Headers;
using DevagramCShrap.Dtos;

namespace DevagramCShrap.Services
{
    public class CosmicService
    {
        public string EnviarImagem(ImagemDto imagemdto)
        {
            Stream imagem;
            imagem = imagemdto.Imagem.OpenReadStream();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "C9e4Ybzr6LS2cXDhCWY1bD5hic9EoHNn9G5ANOb9cqWxN4W9gh");

            var request = new HttpRequestMessage(HttpMethod.Post, "file");
            var conteudo = new MultipartFormDataContent
            {
                { new StreamContent(imagem), "media", imagemdto.Nome },
            };

            request.Content = conteudo;
            var retornoreq = client.PostAsync("https://workers.cosmicjs.com/v3/buckets/devanews-production/media", request.Content).Result;

            var urlretorno = retornoreq.Content.ReadFromJsonAsync<CosmicRespostaDto>();

            return urlretorno.Result.media.url;
        }
    }
}
