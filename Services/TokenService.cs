using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevagramCShrap.Models;
using Microsoft.IdentityModel.Tokens;

namespace DevagramCShrap.Services
{
    public class TokenService
    {
        public const int TamanhoMinimoDaChave = 32;
        private static readonly TimeSpan Validade = TimeSpan.FromDays(7);
        private readonly byte[] _chave;

        public TokenService(IConfiguration configuration)
        {
            _chave = LerChave(configuration);
        }

        // A chave vem da configuração (variável Jwt__Chave ou user-secrets), nunca do código.
        public static byte[] LerChave(IConfiguration configuration)
        {
            var chave = configuration["Jwt:Chave"];
            if (string.IsNullOrWhiteSpace(chave) || chave.Length < TamanhoMinimoDaChave)
            {
                throw new InvalidOperationException(
                    $"Defina Jwt:Chave com pelo menos {TamanhoMinimoDaChave} caracteres (variável de ambiente Jwt__Chave ou dotnet user-secrets).");
            }
            return Encoding.UTF8.GetBytes(chave);
        }

        public string CriarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                }),
                Expires = DateTime.UtcNow.Add(Validade),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_chave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
