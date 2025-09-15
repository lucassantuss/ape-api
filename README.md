# Ape API

API REST desenvolvida para o Trabalho de Conclusão de Curso (TCC) de Engenharia de Computação na Faculdade Engenheiro Salvador Arena.

---

## Índice

1. [Sobre](#sobre)  
2. [Tecnologias](#tecnologias)  
3. [Arquitetura / Estrutura](#arquitetura--estrutura)  
4. [Instalação e Execução](#instalação-e-execução)  
5. [Deploy / Ambiente de Produção](#deploy--ambiente-de-produção)  
6. [Contribuição](#contribuição)  
7. [Licença](#licença)  

---

## Sobre

Este projeto consiste em uma API REST que serve como backend do TCC, com foco em **operações CRUD**, validações, autenticação e persistência de dados.  

A API está hospedada em: [Swagger da API](https://ape-api.azurewebsites.net/swagger)  

---

## Tecnologias

- Linguagem: **C# / .NET**  
- Banco de dados: **MongoDB**  
- Frameworks e bibliotecas principais: **ASP.NET Core**, bibliotecas de conexão com MongoDB, documentação **Swagger/OpenAPI**  
- Licença: **MIT**

---

## Arquitetura / Estrutura

Estrutura típica de projeto:

/Ape.API

├── Bll/ # Lógica de negócio

├── Controllers/ # Definição dos endpoints API

├── Database/ # Acesso aos dados / banco

├── DTOs/ # Objetos de transferência

├── Entity/ # Modelos de dados

└── Properties/ Config # Arquivos de configuração (appsettings, conexão com BD etc.)

├── Program # Configuração da aplicação (middleware, rotas etc.)

---

## Instalação e Execução

### Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/) (net6.0)  
- MongoDB em execução (local ou remoto)  
- Variáveis de ambiente e configurações de conexão definidas (string de conexão com o MongoDB etc.)

### Passos

1. Clonar este repositório  
   ```bash
   git clone https://github.com/lucassantuss/ape-api.git
   cd ape-api

2. Restaurar pacotes
    ```bash
    dotnet restore

3. Ajustar configurações (appsettings.json ou variáveis de ambiente) com a string de conexão do MongoDB

4. Iniciar a aplicação
    ```bash
    dotnet run

5. Acessar Swagger para testar os endpoints:
    ```bash
    http://localhost:<porta>/swagger

---

## Deploy / Ambiente de Produção

Hospedagem atual: Azure (App Service)

Configurações de ambiente: variáveis de conexão com banco, CORS, certificados, segurança, etc.

---

## Contribuição

Contribuições são bem-vindas! Para colaborar:

- Fork este repositório
- Crie uma nova branch para sua feature ou correção (feature/nome-da-feature)
- Realize as alterações
- Submeta um Pull Request descrevendo as mudanças

---

## Licença

Este projeto está licenciado sob a licença MIT.

Veja o arquivo [MIT](LICENSE) para mais detalhes.