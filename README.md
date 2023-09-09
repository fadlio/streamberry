# Streamberry

## Streamberry.Context
DataLayer utilizado para comunicação com o banco de dados. Por simplicidade, SQLite foi utilizado no projeto. É possível setar connection strings diferentes para o ambiente de desenvolvimento e produção.
Quando iniciado no ambiente de desenvolvimento, o banco é reinicializado com dados de testes.


[EntityFramework](https://github.com/dotnet/efcore) foi utilizado para controle de migrações e para as iterações mais simples com o banco de dados. [Dapper](https://github.com/DapperLib/Dapper) foi utlizado quando as queries geradas pelo EF eram ineficientes, possibilitando definir as queries de forma manual.

## Streamberry.Rest
API com autenticação básica via JWT e endpoints REST para operações CRUD. 
Quando em ambiente de desenvolvimento, Swagger é utilizado para documentação crua das rotas.

## Streamberry.Gql
API GraphQL, exclusiva para leituras, utilizando a plataforma [ChilliCream](https://github.com/ChilliCream/graphql-platform) (Hot Chocolate e Banana Cake Pop).

## Comandos
### Streamberry
 - `dotnet tool restore`: Instala ferramentas de desenvolvimento (EF)
 
### Streamberry.Rest
- `dotnet run`: Inicia API

### Streamberry.Gql
- `dotnet run`: Inicia API

### Streamberry.Context
- `dotnet ef migrations add {NAME}`: Cria migração
- `dotnet ef migrations remove`: Remove última migração