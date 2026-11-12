# Devagram — API (C#)

API de uma rede social de fotos, no estilo Instagram: usuários se cadastram, publicam fotos, seguem outros usuários, curtem, comentam e favoritam publicações.

Construída com ASP.NET Core e SQL Server. Nasceu como projeto da formação Devaria e foi revisada para servir de referência: senhas com argon2, nenhum segredo no código e documentação OpenAPI navegável.

## Stack

| Camada       | Tecnologia                                          |
| ------------ | --------------------------------------------------- |
| Runtime      | .NET 10, ASP.NET Core                               |
| Banco        | SQL Server com Entity Framework Core 10             |
| Autenticação | JWT (HS256, validade de 7 dias), senhas em argon2id |
| Documentação | Swagger (Swashbuckle) e Scalar                      |
| Imagens      | Cosmic                                              |
| Testes       | xUnit                                               |

## Como rodar

```bash
dotnet user-secrets set "Jwt:Chave" "uma-chave-com-pelo-menos-32-caracteres"
```

```bash
dotnet user-secrets set "Cosmic:Bucket" "slug-do-bucket"
```

```bash
dotnet user-secrets set "Cosmic:Token" "chave-de-escrita-do-bucket"
```

```bash
dotnet tool install --global dotnet-ef
```

```bash
dotnet ef database update
```

```bash
dotnet run
```

A string de conexão padrão, em `appsettings.json`, aponta para um SQL Server local com autenticação integrada. Para outro servidor, defina `ConnectionStrings:DefaultConnection` da mesma forma.

| Endereço                   | O que é                        |
| -------------------------- | ------------------------------ |
| `/scalar`                  | Referência interativa (Scalar) |
| `/swagger`                 | Swagger UI                     |
| `/swagger/v1/swagger.json` | Documento OpenAPI              |

### Configuração

Fora do ambiente de desenvolvimento, use variáveis de ambiente (`__` no lugar de `:`).

| Chave                                  | Variável de ambiente                   | Obrigatória | Para quê                                     |
| -------------------------------------- | -------------------------------------- | ----------- | -------------------------------------------- |
| `Jwt:Chave`                            | `Jwt__Chave`                           | sim         | Segredo que assina os tokens (32+ caracteres) |
| `ConnectionStrings:DefaultConnection`  | `ConnectionStrings__DefaultConnection` | sim         | Conexão com o SQL Server                     |
| `Cosmic:Bucket`                        | `Cosmic__Bucket`                       | para upload | Slug do bucket no Cosmic                     |
| `Cosmic:Token`                         | `Cosmic__Token`                        | para upload | Chave de escrita do bucket                   |

A API não sobe sem `Jwt:Chave`: a falha aparece na inicialização, não na primeira requisição.

## Rotas

| Rota                                   | Método   | O que faz                                    |
| -------------------------------------- | -------- | -------------------------------------------- |
| `/api/Login`                           | POST     | Entrar e receber o token (pública)           |
| `/api/Usuario`                         | POST     | Criar conta, com foto opcional (pública)     |
| `/api/Usuario`                         | GET, PUT | Ver e atualizar o próprio cadastro           |
| `/api/Usuario/pesquisaid`              | GET      | Ver um usuário e os contadores dele          |
| `/api/Usuario/pesquisanome`            | GET      | Buscar usuários por nome                     |
| `/api/Publicacao`                      | POST     | Publicar uma foto                            |
| `/api/Publicacao/feed`                 | GET      | Feed de quem o usuário segue                 |
| `/api/Publicacao/feedusuario`          | GET      | Publicações de um usuário                    |
| `/api/Publicacao/favoritas`            | GET      | Publicações favoritadas                      |
| `/api/Publicacao/compartilhar`         | POST     | Compartilhar uma publicação                  |
| `/api/Comentario`                      | PUT      | Comentar                                     |
| `/api/Curtida`                         | PUT      | Curtir ou descurtir                          |
| `/api/Favoritar`                       | GET, PUT | Listar favoritas e favoritar ou desfavoritar |
| `/api/Seguidor`                        | PUT      | Seguir ou deixar de seguir                   |

Fora login e cadastro, todas esperam `Authorization: Bearer <token>`.

## Estrutura

```
Controllers/  rotas da API
Services/     senha, token e envio de imagens
Repository/   acesso ao banco (interfaces e implementações)
Models/       entidades e contexto do Entity Framework
Dtos/         formatos de entrada e saída
Migrations/   migrations do Entity Framework
tests/        testes unitários
```

## Testes

```bash
dotnet test tests/DevagramCShrap.Tests
```

## Segurança

- Senhas gravadas com argon2id. Contas antigas, em MD5, continuam entrando e têm a senha regravada em argon2 no primeiro login.
- Chave do JWT e credenciais do Cosmic vêm da configuração; antes estavam fixas no código. Tokens assinados com a chave antiga não são mais aceitos.
- Token com validade de 7 dias e algoritmo fixo.
- Upload aceita só imagens de até 5 MB.
- Erros inesperados respondem com mensagem genérica; o detalhe fica no log do servidor.
