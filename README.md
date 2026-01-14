# 🍔 TechVeo Shared Libraries

[![Build](https://github.com/FIAP-Hackathon-Team/techVeo-shared/actions/workflows/pipeline.yaml/badge.svg)](https://github.com/FIAP-Hackathon-Team/techVeo-shared/actions/workflows/pipeline.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Biblioteca compartilhada de componentes reutilizáveis para o ecossistema TechVeo, distribuída como pacotes NuGet privados no GitHub Packages.

## 📦 Pacotes

Este repositório contém 4 pacotes NuGet:

| Pacote                          | Descrição                                               | Versão                                                                                                          |
| ------------------------------- | ------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| **TechVeo.Shared.Domain**       | Entidades, enums, value objects e validações de domínio | [![NuGet](https://img.shields.io/badge/nuget-1.0.0-blue)](https://github.com/orgs/FIAP-Hackathon-Team/packages) |
| **TechVeo.Shared.Application**  | Exceções, extensões e recursos de aplicação             | [![NuGet](https://img.shields.io/badge/nuget-1.0.0-blue)](https://github.com/orgs/FIAP-Hackathon-Team/packages) |
| **TechVeo.Shared.Infra**        | Persistência, Entity Framework e consistência eventual  | [![NuGet](https://img.shields.io/badge/nuget-1.0.0-blue)](https://github.com/orgs/FIAP-Hackathon-Team/packages) |
| **TechVeo.Shared.Presentation** | Filtros, extensões ASP.NET Core e configurações Swagger | [![NuGet](https://img.shields.io/badge/nuget-1.0.0-blue)](https://github.com/orgs/FIAP-Hackathon-Team/packages) |

## 🚀 Quick Start

### Instalação

```bash
# Adicionar source do GitHub Packages
dotnet nuget add source https://nuget.pkg.github.com/FIAP-Hackathon-Team/index.json \
  --name github \
  --username SEU_USUARIO \
  --password SEU_TOKEN \
  --store-password-in-clear-text

# Instalar pacotes
dotnet add package TechVeo.Shared.Domain
dotnet add package TechVeo.Shared.Application
dotnet add package TechVeo.Shared.Infra
dotnet add package TechVeo.Shared.Presentation
```

### Uso Básico

```csharp
using TechVeo.Shared.Domain.Enums;
using TechVeo.Shared.Application.Exceptions;
using TechVeo.Shared.Infra.UoW;
using TechVeo.Shared.Presentation.Extensions;

// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTechVeoSwagger();
builder.Services.AddTechVeoPersistence(builder.Configuration);

var app = builder.Build();
app.UseTechVeoRequestPipeline();
app.MapControllers();
app.Run();
```

## 📚 Documentação

- **[NUGET.md](NUGET.md)** - Guia completo de configuração e uso dos pacotes
- **[QUICKSTART.md](QUICKSTART.md)** - Guia rápido de publicação e versionamento
- **[EXAMPLES.md](EXAMPLES.md)** - Exemplos práticos de uso

## 🏗️ Estrutura do Projeto

```
techVeo-shared/
├── src/
│   ├── TechVeo.Shared.Domain/          # Camada de domínio
│   │   ├── Common/
│   │   │   ├── Entities/                # Entidades base
│   │   │   ├── ValueObjects/            # Value Objects (CPF, Email, etc)
│   │   │   ├── Validations/             # Validações de domínio
│   │   │   └── Exceptions/              # Exceções de domínio
│   │   └── Enums/                       # Enumeradores
│   │
│   ├── TechVeo.Shared.Application/     # Camada de aplicação
│   │   ├── Exceptions/                  # Exceções de aplicação
│   │   └── Extensions/                  # Métodos de extensão
│   │
│   ├── TechVeo.Shared.Infra/          # Camada de infraestrutura
│   │   ├── Persistence/                 # Configurações EF Core
│   │   ├── EventualConsistency/         # Consistência eventual
│   │   └── UoW/                         # Unit of Work
│   │
│   └── TechVeo.Shared.Presentation/    # Camada de apresentação
│       ├── Extensions/                  # Extensões ASP.NET Core
│       ├── Filters/                     # Filtros globais
│       └── NamingPolicy/                # Políticas de nomenclatura JSON
│
├── tests/                               # Testes unitários
├── .github/workflows/                   # CI/CD Pipeline
└── docs/                                # Documentação adicional
```

## 🔄 Workflow de Publicação

### Desenvolvimento (Versão Beta)

```bash
git checkout -b feature/nova-funcionalidade
# ... fazer alterações ...
git commit -m "feat: adiciona nova funcionalidade"
git push origin feature/nova-funcionalidade
# Criar PR e fazer merge para main
# Pipeline publica automaticamente versão 1.0.X-beta
```

### Produção (Versão Estável)

```bash
git checkout main
git pull origin main
git tag v1.0.0
git push origin v1.0.0
# Pipeline publica versão 1.0.0 + cria GitHub Release
```

## 🛠️ Desenvolvimento Local

### Pré-requisitos

- .NET 8.0 SDK
- Git

### Build e Test

```bash
# Restaurar dependências
dotnet restore TechVeo.Shared.sln

# Build
dotnet build TechVeo.Shared.sln --configuration Release

# Executar testes
dotnet test TechVeo.Shared.sln --configuration Release
```

### Criar Pacotes Localmente

```powershell
# Windows (PowerShell)
.\pack-local.ps1 -Version "1.0.0-dev"
```

```bash
# Linux/Mac
./pack-local.sh 1.0.0-dev
```

## 📋 Recursos Principais

### Domain

- ✅ Entidades base com auditoria (CreatedAt, UpdatedAt, DeletedAt)
- ✅ Value Objects validados (CPF, Email, Telefone)
- ✅ Enums de negócio (OrderStatus, PaymentType, etc)
- ✅ Exceções de domínio tipadas
- ✅ Validações reutilizáveis

### Application

- ✅ Exceções customizadas (NotFoundException, ApplicationException)
- ✅ Extensions para ClaimsPrincipal (GetUserId, GetUserEmail)
- ✅ Extensions LINQ (Paginate, WhereIf)
- ✅ Recursos localizados

### Infrastructure

- ✅ Unit of Work pattern
- ✅ Base Repositories com EF Core
- ✅ Configuração de contexto reutilizável
- ✅ Suporte a consistência eventual
- ✅ Interceptors e extensões

### Presentation

- ✅ Filtros globais (Validation, Exception, Logging)
- ✅ Configuração Swagger padronizada
- ✅ JWT Authentication helpers
- ✅ Naming policies customizados
- ✅ Pipeline de request configurado

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Com coverage
dotnet test --collect:"XPlat Code Coverage"

# Gerar relatório de coverage
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:Html
```

## 📊 CI/CD Pipeline

A pipeline automatiza:

- ✅ Build e compilação
- ✅ Execução de testes
- ✅ Validação de coverage (mínimo 50%)
- ✅ Criação de pacotes NuGet
- ✅ Publicação no GitHub Packages
- ✅ Criação de releases no GitHub

## 🔐 Segurança

- Pacotes privados (requer autenticação)
- Token PAT com escopo `read:packages`
- Credenciais nunca commitadas no repositório
- Uso de secrets no GitHub Actions

## 🤝 Contribuindo

1. Fork o repositório
2. Crie uma branch: `git checkout -b feature/minha-feature`
3. Commit: `git commit -m 'feat: minha nova feature'`
4. Push: `git push origin feature/minha-feature`
5. Abra um Pull Request

### Convenção de Commits

Seguimos [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` Nova funcionalidade
- `fix:` Correção de bug
- `docs:` Documentação
- `style:` Formatação
- `refactor:` Refatoração
- `test:` Testes
- `chore:` Manutenção

## 📄 Licença

Este projeto está sob a licença MIT. Veja [LICENSE](LICENSE) para mais detalhes.

## 🔗 Links Úteis

- [GitHub Packages](https://github.com/orgs/FIAP-Hackathon-Team/packages)
- [Pipeline Actions](https://github.com/FIAP-Hackathon-Team/techVeo-shared/actions)
- [Releases](https://github.com/FIAP-Hackathon-Team/techVeo-shared/releases)

## 💬 Suporte

Para questões ou problemas:

- Abra uma [Issue](https://github.com/FIAP-Hackathon-Team/techVeo-shared/issues)
- Consulte a [Documentação](./NUGET.md)
- Entre em contato com a equipe

---

Feito com ❤️ pela FIAP-Hackathon-Team Team
