# Advanced Business Development with .NET

## Integrantes do Projeto

- **Victor Fanfoni** RM-99173
- **Helena Paixão** RM-550929
- **Gustavo Costa** RM-99102
- **Julia Nery** RM-552292
- **Giulia Pina** RM-97694

## Descrição do Projeto

O **Advanced Business Development with .NET** é uma aplicação de API RESTful desenvolvida para melhorar os processos de gestão de energia sustentável. O projeto foca em modularidade, escalabilidade e separação de responsabilidades, seguindo as melhores práticas de Arquitetura de Software. A aplicação incorpora design patterns como **Repository**, **Factory** e **Dependency Injection**, garantindo um código bem organizado e fácil de manter. Além disso, a API é documentada com **Swagger** e testada utilizando **xUnit** e **Moq**, com o objetivo de fornecer uma solução robusta e de alta qualidade.

### Funcionalidades Principais

- **Monitoramento de Consumo de Energia**: Coleta e apresenta dados sobre o consumo de energia de dispositivos e fontes sustentáveis.
- **Previsão de Consumo de Energia**: Utiliza IA generativa para prever padrões de consumo, ajudando na otimização do uso de energia.
- **Gestão de Dados de Energia**: Implementação de endpoints CRUD para manipulação e visualização dos dados relacionados ao consumo de energia.
- **Testes Automatizados**: A API possui cobertura de testes unitários, integração e sistema utilizando **xUnit** e **Moq**, garantindo alta qualidade e confiabilidade.
- **Banco de Dados**: O projeto utiliza **Oracle**, conforme a necessidade de persistência dos dados.

## Arquitetura

A arquitetura do projeto segue os princípios do **Clean Architecture**, garantindo que o sistema seja organizado e de fácil manutenção. As principais camadas são:

- **Controller**: Responsável pelo recebimento de requisições HTTP e retorno das respostas.
- **Service**: Contém a lógica de negócios da aplicação.
- **Repository**: Interage com o banco de dados para manipulação e persistência de dados.
- **Models**: Define as entidades e objetos de dados utilizados no sistema.
- **Data**: Contém a configuração do contexto de banco de dados e as migrações.

### Design Patterns Utilizados

- **Repository**: Responsável pela abstração do acesso a dados, facilitando a manutenção e testabilidade.
- **Factory**: Implementado para a criação de objetos de forma desacoplada, promovendo flexibilidade na adição de novos tipos de objetos.
- **Dependency Injection**: Utilizado para promover o desacoplamento entre as classes, facilitando a escalabilidade e a manutenção do sistema.

## Justificativa da Arquitetura

A escolha da arquitetura modular, com uso de design patterns, garante uma estrutura de código mais limpa, fácil de testar e manter. O uso de **Dependency Injection** facilita a injeção de dependências e promove o desacoplamento entre os componentes. Isso resulta em uma aplicação escalável e robusta, capaz de lidar com novos requisitos e mudanças com maior facilidade.

## Tecnologias Utilizadas

- **.NET Core 8.0**: Plataforma de desenvolvimento da API.
- **Swagger**: Para documentação e testes da API.
- **xUnit e Moq**: Para testes automatizados e mock de dependências.
- **Oracle**: Para persistência de dados, dependendo dos requisitos de escalabilidade e flexibilidade.
- **Hugging Face**: Implementação de IA generativa para prever padrões de consumo de energia.

## Como Executar o Projeto

### Pré-requisitos

- **.NET Core 8.0 ou superior**
- **Oracle** configurado e acessível
- Dependências do projeto:

```bash
dotnet restore
