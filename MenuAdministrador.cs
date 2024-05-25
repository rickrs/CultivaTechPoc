using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace CultivaTechPoc
{
    internal class MenuAdministrador
    {
        private readonly string _connectionString;

        public MenuAdministrador()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("CultivaTechDB");
        }

        public void Exibir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("|         Menu do Administrador     |");
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("| 1 - Gerenciamento de Produção     |");
                Console.WriteLine("| 2 - Simulação de Vendas           |");
                Console.WriteLine("| 3 - Relatório de Produção Total   |");
                Console.WriteLine("| 4 - Relatório de Insumos Utilizados|");
                Console.WriteLine("| 5 - Gerenciamento de Estoque      |");
                Console.WriteLine("| 6 - Gerenciamento de Fornecedores |");
                Console.WriteLine("| 7 - Gerenciamento de Usuários     |");
                Console.WriteLine("| 8 - Gerar Script SQL              |");
                Console.WriteLine("| 0 - Sair                          |");
                Console.WriteLine("+-----------------------------------+");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine();

                if (opcao == "0")
                {
                    break;
                }

                switch (opcao)
                {
                    case "1":
                        GerenciamentoProducao();
                        break;
                    case "2":
                        SimulacaoVendas();
                        break;
                    case "3":
                        RelatorioProducaoTotal();
                        break;
                    case "4":
                        RelatorioInsumosUtilizados();
                        break;
                    case "5":
                        GerenciamentoEstoque();
                        break;
                    case "6":
                        GerenciamentoFornecedores();
                        break;
                    case "7":
                        GerenciamentoUsuarios();
                        break;
                    case "8":
                        GerarScriptSQL();
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                        Console.ReadKey();
                        break;
                }

                Console.WriteLine("Pressione qualquer tecla para voltar ao menu.");
                Console.ReadKey();
            }

            // Quando o loop termina, volta para o login
            Login login = new Login();
            login.Entrar();
        }

        private void GerenciamentoProducao()
        {
            GerenciamentoProducao producao = new GerenciamentoProducao();
            producao.Exibir();
        }

        private void SimulacaoVendas()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|        Simulação de Vendas        |");
            Console.WriteLine("+-----------------------------------+");
            // Adicione aqui a lógica para Simulação de Vendas
            Console.WriteLine("Funcionalidade em desenvolvimento...");
        }

        private void RelatorioProducaoTotal()
        {
            RelatorioProducao relatorio = new RelatorioProducao();
            relatorio.ExibirRelatorio();
        }

        private void RelatorioInsumosUtilizados()
        {
            RelatorioInsumosUtilizados relatorioinsumo = new RelatorioInsumosUtilizados();
            relatorioinsumo.ExibirRelatorio();
        }

        private void GerenciamentoEstoque()
        {
            GerenciamentoEstoque estoque = new GerenciamentoEstoque();
            estoque.Exibir();
        }

        private void GerenciamentoFornecedores()
        {
            GerenciamentoFornecedores fornecedores = new GerenciamentoFornecedores();
            fornecedores.Exibir();
        }

        private void GerenciamentoUsuarios()
        {
            UsuarioCRUD usuarioCRUD = new UsuarioCRUD(_connectionString);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("|      Gerenciamento de Usuários    |");
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("| 1 - Adicionar Usuário             |");
                Console.WriteLine("| 2 - Listar Usuários               |");
                Console.WriteLine("| 3 - Editar Usuário                |");
                Console.WriteLine("| 4 - Remover Usuário               |");
                Console.WriteLine("| 0 - Voltar                        |");
                Console.WriteLine("+-----------------------------------+");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine();

                if (opcao == "0")
                {
                    break;
                }

                switch (opcao)
                {
                    case "1":
                        AdicionarUsuario(usuarioCRUD);
                        break;
                    case "2":
                        ListarUsuarios(usuarioCRUD);
                        break;
                    case "3":
                        EditarUsuario(usuarioCRUD);
                        break;
                    case "4":
                        RemoverUsuario(usuarioCRUD);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                        Console.ReadKey();
                        break;
                }

                Console.WriteLine("Pressione qualquer tecla para continuar.");
                Console.ReadKey();
            }
        }

        private void AdicionarUsuario(UsuarioCRUD usuarioCRUD)
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|         Adicionar Usuário         |");
            Console.WriteLine("+-----------------------------------+");

            Usuarios usuario = new Usuarios();
            Console.Write("Nome: ");
            usuario.Nome = Console.ReadLine();
            Console.Write("Email: ");
            usuario.Email = Console.ReadLine();
            Console.Write("Senha: ");
            usuario.Senha = Console.ReadLine();
            Console.Write("Tipo (Admin/Gestor): ");
            usuario.Tipo = Console.ReadLine();

            usuarioCRUD.AdicionarUsuario(usuario);
            Console.WriteLine("Usuário adicionado com sucesso!");
        }

        private void ListarUsuarios(UsuarioCRUD usuarioCRUD)
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|          Listar Usuários          |");
            Console.WriteLine("+-----------------------------------+");

            var usuarios = usuarioCRUD.ListarUsuarios();
            foreach (var usuario in usuarios)
            {
                Console.WriteLine($"ID: {usuario.Id}");
                Console.WriteLine($"Nome: {usuario.Nome}");
                Console.WriteLine($"Email: {usuario.Email}");
                Console.WriteLine($"Tipo: {usuario.Tipo}");
                Console.WriteLine("+-----------------------------------+");
            }
        }

        private void EditarUsuario(UsuarioCRUD usuarioCRUD)
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|          Editar Usuário           |");
            Console.WriteLine("+-----------------------------------+");

            Console.Write("ID do Usuário: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var usuarios = usuarioCRUD.ListarUsuarios();
            var usuario = usuarios.Find(u => u.Id == id);

            if (usuario == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return;
            }

            Console.Write($"Nome ({usuario.Nome}): ");
            string nome = Console.ReadLine();
            Console.Write($"Email ({usuario.Email}): ");
            string email = Console.ReadLine();
            Console.Write("Senha (deixe em branco para manter): ");
            string senha = Console.ReadLine();
            Console.Write($"Tipo ({usuario.Tipo}): ");
            string tipo = Console.ReadLine();

            if (!string.IsNullOrEmpty(nome)) usuario.Nome = nome;
            if (!string.IsNullOrEmpty(email)) usuario.Email = email;
            if (!string.IsNullOrEmpty(senha)) usuario.Senha = senha;
            if (!string.IsNullOrEmpty(tipo)) usuario.Tipo = tipo;

            usuarioCRUD.AtualizarUsuario(usuario);
            Console.WriteLine("Usuário atualizado com sucesso!");
        }

        private void RemoverUsuario(UsuarioCRUD usuarioCRUD)
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|          Remover Usuário          |");
            Console.WriteLine("+-----------------------------------+");

            Console.Write("ID do Usuário: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            usuarioCRUD.RemoverUsuario(id);
            Console.WriteLine("Usuário removido com sucesso!");
        }
        private void GerarScriptSQL()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|       Gerar Script SQL            |");
            Console.WriteLine("+-----------------------------------+");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "script_geracao_insercao.sql");

            try
            {
                // Cria a conexão com o banco de dados
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    ServerConnection serverConnection = new ServerConnection(connection);
                    Server server = new Server(serverConnection);
                    Database database = server.Databases["CultivaTech"];

                    Scripter scripter = new Scripter(server)
                    {
                        Options =
                        {
                            ScriptDrops = false,
                            ScriptSchema = true,
                            ScriptData = true,
                            ScriptBatchTerminator = true,
                            Indexes = true,
                            Triggers = true,
                            FullTextIndexes = true,
                            ExtendedProperties = true,
                            
                        }
                    };

                    StringWriter stringWriter = new StringWriter();

                    foreach (Table table in database.Tables)
                    {
                        if (!table.IsSystemObject)
                        {
                            // Script de criação da tabela
                            foreach (string script in scripter.EnumScript(new Urn[] { table.Urn }))
                            {
                                stringWriter.WriteLine(script);
                            }

                            // Script de inserção de dados
                            foreach (string script in scripter.EnumScript(new Urn[] { table.Urn }))
                            {
                                stringWriter.WriteLine(script);
                            }
                        }
                    }

                    File.WriteAllText(filePath, stringWriter.ToString());
                }

                Console.WriteLine($"Script SQL gerado com sucesso em: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar o script SQL: {ex.Message}");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu.");
            Console.ReadKey();
        }
    }
}
