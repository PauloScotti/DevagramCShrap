using DevagramCShrap.Services;
using Xunit;

namespace DevagramCShrap.Tests
{
    public class SenhaServiceTests
    {
        [Fact]
        public void GeraHashArgon2DiferenteACadaChamada()
        {
            var primeiro = SenhaService.GerarHash("segredo-forte");
            var segundo = SenhaService.GerarHash("segredo-forte");

            Assert.StartsWith("$argon2id$v=19$", primeiro);
            Assert.NotEqual(primeiro, segundo);
            Assert.DoesNotContain("segredo-forte", primeiro);
        }

        [Fact]
        public void ConfereSenhaCertaERejeitaErrada()
        {
            var armazenada = SenhaService.GerarHash("segredo-forte");

            Assert.Equal((true, false), SenhaService.Conferir("segredo-forte", armazenada));
            Assert.Equal((false, false), SenhaService.Conferir("errada", armazenada));
        }

        [Fact]
        public void SenhaAntigaEmMd5EntraEPedeParaRegravar()
        {
            var armazenada = SenhaService.HashMd5Legado("1234");

            Assert.Equal((true, true), SenhaService.Conferir("1234", armazenada));
            Assert.Equal((false, false), SenhaService.Conferir("4321", armazenada));
        }

        [Fact]
        public void HashLegadoMantemOFormatoQueJaEstaNoBanco()
        {
            // MD5 de "1234" = 81 dc 9b db 52 d0 4d c2 00 36 db d8 31 3e d0 55, cada byte em decimal.
            Assert.Equal("1292201552198220877194054219216496220885", SenhaService.HashMd5Legado("1234"));
        }

        [Fact]
        public void OProprioValorArmazenadoNaoServeComoSenha()
        {
            var argon = SenhaService.GerarHash("segredo-forte");
            var md5 = SenhaService.HashMd5Legado("1234");

            Assert.False(SenhaService.Conferir(argon, argon).Valida);
            Assert.False(SenhaService.Conferir(md5, md5).Valida);
        }

        [Theory]
        [InlineData("")]
        [InlineData("$argon2id$")]
        [InlineData("$argon2id$v=19$m=x,t=3,p=1$abc$def")]
        [InlineData("$argon2id$v=19$m=65536,t=3,p=1$%%%$%%%")]
        public void ValorArmazenadoMalformadoNaoEstoura(string armazenada)
        {
            Assert.Equal((false, false), SenhaService.Conferir("1234", armazenada));
        }
    }
}
