using DevagramCShrap.Models;
using DevagramCShrap.Repository;
using DevagramCShrap.Repository.Impl;
using DevagramCShrap.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DevagramContext>(option => option.UseSqlServer(connectionstring));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositoryImpl>();
builder.Services.AddScoped<ISeguidorRepository, SeguidorRepositoryImpl>();
builder.Services.AddScoped<IPublicacaoRepository, PublicacaoRepositoryImpl>();
builder.Services.AddScoped<IComentarioRepository, ComentarioRepositoryImpl>();
builder.Services.AddScoped<ICurtidaRepository, CurtidaRepositoryImpl>();
builder.Services.AddScoped<IFavoritarRepository, FavoritarRepositoryImpl>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddHttpClient<CosmicService>();

// Falha na subida, e não na primeira requisição, se a chave não estiver configurada.
var chaveCriptografia = TokenService.LerChave(builder.Configuration);
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(autenticacao =>
{
    autenticacao.SaveToken = true;
    autenticacao.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chaveCriptografia),
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Swagger UI em /swagger e referência Scalar em /scalar.
app.UseSwagger();
app.UseSwaggerUI();
app.MapScalarApiReference(opcoes => opcoes
    .WithTitle("Devagram API")
    .WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
