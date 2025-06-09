# API de Gestão de Segurança

Sistema robusto para gerenciamento de segurança urbana que oferece controle completo sobre usuários, rotas e análise de riscos através de uma interface RESTful moderna.

##  Visão Geral

A aplicação foi desenvolvida com foco em escalabilidade e performance, oferecendo recursos avançados como predição inteligente de riscos via machine learning, processamento assíncrono de mensagens e proteção contra sobrecarga.

### Principais Funcionalidades

- **Gestão Completa de Usuários**: Cadastro, atualização e controle de acesso
- **Mapeamento de Rotas Seguras**: Sistema de coordenadas geográficas para navegação
- **Análise Preditiva de Riscos**: Machine learning para classificação automática de ameaças
- **Processamento Assíncrono**: Comunicação via filas RabbitMQ
- **Proteção Anti-Spam**: Rate limiting configurável por endpoint
- **Navegação Intuitiva**: Implementação HATEOAS para descoberta de recursos

##  Stack Tecnológica

### Backend
- **Runtime**: .NET 6
- **Linguagem**: C#
- **Framework Web**: ASP.NET Core

### Persistência
- **Banco de Dados**: Oracle Database
- **ORM**: Entity Framework Core
- **Provider**: Oracle.EntityFrameworkCore

### Integrações
- **Message Broker**: RabbitMQ
- **Machine Learning**: ML.NET
- **Serialização**: Newtonsoft.Json

### Qualidade e Documentação
- **Testes**: xUnit Framework
- **API Docs**: Swagger/OpenAPI
- **Rate Limiting**: ASP.NET Core Rate Limit

##  Pré-requisitos

Certifique-se de ter os seguintes componentes instalados:

- .NET 6 SDK
- Oracle Database (local ou via Docker)
- RabbitMQ Server
- Git (recomendado)

##  Configuração e Instalação

### 1. Preparação do Ambiente

```bash
# Clone o repositório
git clone [URL_DO_REPOSITORIO]
cd [NOME_DO_PROJETO]

# Restaure as dependências
dotnet restore
```

### 2. Configuração de Banco de Dados

Configure sua string de conexão Oracle no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "sua-connection-string-oracle"
  }
}
```

### 3. Inicialização da Aplicação

```bash
# Executar localmente
dotnet run

# Ou via Docker Compose
docker-compose up -d
dotnet run
```

### 4. Verificação da Instalação

Acesse a documentação interativa em: `http://localhost:5000/swagger`

##  Referência da API

### Módulo de Usuários

**Base URL**: `/api/usuarios`

| Verbo HTTP | Rota | Função |
|------------|------|---------|
| `GET` | `/` | Recupera lista paginada de usuários |
| `GET` | `/{id}` | Busca usuário específico por ID |
| `POST` | `/` | Registra novo usuário no sistema |
| `PUT` | `/{id}` | Modifica dados de usuário existente |
| `DELETE` | `/{id}` | Remove usuário permanentemente |

### Módulo de Rotas Seguras

**Base URL**: `/api/rotas-seguras`

| Verbo HTTP | Rota | Função |
|------------|------|---------|
| `GET` | `/` | Lista todas as rotas cadastradas |
| `GET` | `/{id}` | Consulta rota específica |
| `POST` | `/` | Cadastra nova rota segura |
| `PUT` | `/{id}` | Atualiza informações da rota |
| `DELETE` | `/{id}` | Desativa rota do sistema |

### Módulo de Análise de Riscos

**Base URL**: `/api/riscos`

| Verbo HTTP | Rota | Função |
|------------|------|---------|
| `GET` | `/` | Exibe relatório de riscos |
| `GET` | `/{id}` | Detalha risco específico |
| `POST` | `/` | Cria registro com predição ML |
| `PUT` | `/{id}` | Atualiza avaliação de risco |
| `DELETE` | `/{id}` | Remove registro de risco |

##  Recursos Avançados

### HATEOAS (Hypermedia as the Engine of Application State)
Todas as respostas incluem links de navegação para ações relacionadas, facilitando a descoberta de funcionalidades.

### Rate Limiting Inteligente
Proteção configurada para 5 requisições por segundo em endpoints de criação, evitando sobrecarga do sistema.

### Predição por Machine Learning
O sistema utiliza ML.NET para classificar automaticamente o nível de severidade dos riscos reportados.

##  Estratégias de Teste

### Execução de Testes Automatizados

```bash
# Todos os testes
dotnet test

# Testes com relatório de cobertura
dotnet test --collect:"XPlat Code Coverage"

# Testes de uma categoria específica
dotnet test --filter Category=Unit
```

### Testes Manuais via Interface Swagger

1. Navegue até `http://localhost:5000/swagger`
2. Selecione o endpoint desejado
3. Clique em "Try it out"
4. Configure os parâmetros necessários
5. Execute e analise o resultado

##  Exemplos de Uso

### Cadastro de Usuário

```json
{
  "nome": "João Santos",
  "email": "joao.santos@exemplo.com",
  "perfil": "administrador"
}
```

### Registro de Rota Segura

```json
{
  "nome": "Rota Centro-Aeroporto",
  "localizacao": "Avenida Principal",
  "coordenadas": "-23.550520, -46.633308",
  "nivelSeguranca": "Alto"
}
```

### Reporte de Risco

```json
{
  "titulo": "Iluminação Deficiente",
  "descricao": "Postes de luz queimados no quarteirão 200",
  "localizacao": "Rua das Flores, 200",
  "prioridade": "Media"
}
```

## Grupo

#### **Igor Oviedo RM553434**

#### **Cauã Loureiro RM553093**

#### **Thiago Carrillo RM553565**

*Desenvolvido com foco em segurança, performance e escalabilidade.*