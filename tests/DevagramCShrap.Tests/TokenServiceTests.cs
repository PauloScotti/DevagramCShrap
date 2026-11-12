using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DevagramCShrap.Models;
using DevagramCShrap.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DevagramCShrap.Tests
{
    public class TokenServiceTests
    {
        private static IConfiguration Configuracao(string? chave) =>
            new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["Jwt:Chave"] = chave })
                .Build();

        [Fact]
        public void TokenLevaOIdDoUsuarioETemValidade()
        {
            var service = new TokenService(Configuracao(new string('k', 32)));

            var token = new JwtSecurityTokenHandler().ReadJwtToken(
                service.CriarToken(new Usuario { Id = 7, Nome = "Ana" }));

            Assert.Equal("7", token.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
            Assert.True(token.ValidTo > DateTime.UtcNow.AddDays(6));
            Assert.True(token.ValidTo < DateTime.UtcNow.AddDays(8));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ChaveSecretaDoTokenJWT")]
        public void RecusaSubirSemChaveOuComChaveCurta(string? chave)
        {
            Assert.Throws<InvalidOperationException>(() => new TokenService(Configuracao(chave)));
        }
    }
}
