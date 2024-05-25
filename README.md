# CultivaTechPoc

CultivaTechPoc é uma Prova de Conceito (POC) desenvolvida para gerenciar a produção agrícola, fornecedores, insumos, estoque e vendas. Este projeto demonstra a integração de várias funcionalidades em um sistema console baseado em C# com conexão a um banco de dados Microsoft SQL Server.

## Funcionalidades

1. **Login**
   - Autenticação de usuário com diferentes tipos de acesso: Administrador e Gestor de Vendas.

2. **Menu do Administrador**
   - Gerenciamento de Produção
     - Nova Produção
     - Ativar Plantação
     - Listar Plantações em Andamento
     - Listar Plantações Aguardando Insumos
     - Listar Plantações Finalizadas
   - Simulação de Vendas
   - Relatório de Produção Total
   - Relatório de Insumos Utilizados
   - Gerenciamento de Estoque
     - Adicionar Insumo
     - Listar Insumos
     - Editar Insumo
     - Remover Insumo
     - Adicionar Produto
   - Gerenciamento de Fornecedores
     - Adicionar Fornecedor
     - Listar Fornecedores
     - Editar Fornecedor
     - Remover Fornecedor
   - Gerenciamento de Usuários
     - Adicionar Usuário
     - Listar Usuários
     - Editar Usuário
     - Remover Usuário
   - Gerar Script SQL

3. **Menu do Gestor de Vendas**
   - Simulação de Vendas
   - Listar Plantações em Andamento
   - Listar Plantações Finalizadas
   - Relatório de Produção Total

## Estrutura do Banco de Dados

O banco de dados é composto pelas seguintes tabelas:

- **Usuarios**: Armazena informações dos usuários do sistema.
- **Fornecedores**: Armazena informações dos fornecedores.
- **Insumos**: Armazena informações dos insumos fornecidos.
- **Producoes**: Armazena informações sobre as produções na fazenda.
- **Plantacoes**: Armazena informações sobre as plantações específicas.
- **PlantacaoInsumos**: Armazena informações sobre os insumos necessários para cada plantação.

## Requisitos

- .NET 6.0 SDK ou superior
- Microsoft SQL Server
- Visual Studio 2022 ou superior
- Pacotes NuGet:
  - Microsoft.Data.SqlClient
  - Microsoft.Extensions.Configuration
  - Microsoft.Extensions.Configuration.Json
  - Microsoft.SqlServer.SqlManagementObjects

## Configuração

1. Clone o repositório:
   ```bash
   git clone https://github.com/username/CultivaTechPoc.git
   cd CultivaTechPoc
