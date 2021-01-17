# core-template-webapi
Aplicação .Net Core 3.1 Web API desenvolvida tendo como cenário um aplicativo fictício chamado "Entrega Futura". A aplicação contém os conceitos essenciais para a criação de Web APIs na plataforma .NET. 

Foi implementado o Design Pattern Repository e Unit Of Work, no entanto este último foi utilizado somente por questões didáticas. Uma vez que se utilize os ORMs atuais não existe vantagem em criar uma implementação personalizada de Unit of Work, pois o próprio DbContext já se comporta como tal.

# Objetivo:
O objetivo desta aplicação é ser um Template Project / Api boilerplate, que poderá ser usado como "kickstart" para o seu projeto, ou então como base referencial para algum dos assuntos tratados.

# Como executar:
- Clonar / baixar o repositório em seu workplace.
- Baixar o .Net Core SDK e o Visual Studio / Code mais recentes.
- Efetuar uma busca por "<--ALTERACAO-->" e ajustar conforme o seu projeto.

# Este projeto contém:

- Design Pattern Repository e Unit Of Work;
- Entity Framework (EF) Core; 
- Persistência em MySql;
- DTO e AutoMapper;
- Páginação;
- JWT (Bearer);
- Política CORS;
- Identity (Inclusive com a criação de campos próprios e mensagens de validação em português);
- Versionamento da API;
- Exemplo de Action Filters;
- Health Checks/Health Checks UI;
- Swagger/Swagger UI;

# TODO
- Autenticação de usuários utilizando Claims;
- Testes Unitários com XUnit;
- Log utilizando o provider elmah.io;
- Publicação utilizando Docker;
- Migração de Asp.Net 3.1 para 5.0;
- Versão do projeto em diferentes arquiteturas;

# Sobre
Este projeto foi desenvolvido por Anderson Hansen sob [MIT license](LICENSE).
