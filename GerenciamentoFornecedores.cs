using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CultivaTechPoc
{
    internal class GerenciamentoFornecedores
    {
        private readonly string _connectionString;

        public GerenciamentoFornecedores()
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
                Console.WriteLine("|  Gerenciamento de Fornecedores    |");
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("| 1 - Adicionar Fornecedor          |");
                Console.WriteLine("| 2 - Listar Fornecedores           |");
                Console.WriteLine("| 3 - Editar Fornecedor             |");
                Console.WriteLine("| 4 - Remover Fornecedor            |");
                Console.WriteLine("| 0 - Voltar ao Menu Principal      |");
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
                        AdicionarFornecedor();
                        break;
                    case "2":
                        ListarFornecedores();
                        break;
                    case "3":
                        EditarFornecedor();
                        break;
                    case "4":
                        RemoverFornecedor();
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

        private void AdicionarFornecedor()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|      Adicionar Fornecedor         |");
            Console.WriteLine("+-----------------------------------+");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Contato (Telefone/Email): ");
            string contato = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Fornecedores (nome, contato) VALUES (@nome, @contato)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@contato", contato);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Fornecedor adicionado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao adicionar fornecedor: " + ex.Message);
                }
            }
        }

        private void ListarFornecedores()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|          Fornecedores             |");
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("| ID | Nome      | Contato          |");
            Console.WriteLine("|----|-----------|------------------|");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Fornecedores";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"| {reader["id"],-2} | {reader["nome"],-10} | {reader["contato"],-16} |");
                }
            }

            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|                                   |");
            Console.WriteLine("| [ID] - Editar                     |");
            Console.WriteLine("| [ID] - Remover                    |");
            Console.WriteLine("| [0] - Voltar ao Menu Principal    |");
            Console.WriteLine("|                                   |");
            Console.WriteLine("+-----------------------------------+");

            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            if (opcao == "0")
            {
                return;
            }
            else
            {
                int fornecedorId;
                if (int.TryParse(opcao, out fornecedorId))
                {
                    Console.WriteLine("| 1 - Editar                        |");
                    Console.WriteLine("| 2 - Remover                       |");
                    Console.WriteLine("+-----------------------------------+");
                    Console.Write("Escolha uma opção: ");
                    string subOpcao = Console.ReadLine();

                    if (subOpcao == "1")
                    {
                        EditarFornecedor();
                    }
                    else if (subOpcao == "2")
                    {
                        RemoverFornecedor();
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("ID inválido. Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                }
            }
        }

        private void EditarFornecedor()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|         Editar Fornecedor         |");
            Console.WriteLine("+-----------------------------------+");

            Console.Write("ID do Fornecedor: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string selectQuery = "SELECT * FROM Fornecedores WHERE id = @id";
                SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();

                if (reader.Read())
                {
                    Console.Write($"Nome ({reader["nome"]}): ");
                    string nome = Console.ReadLine();
                    Console.Write($"Contato ({reader["contato"]}): ");
                    string contato = Console.ReadLine();
                    reader.Close();

                    string updateQuery = "UPDATE Fornecedores SET nome = @nome, contato = @contato WHERE id = @id";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@nome", string.IsNullOrEmpty(nome) ? reader["nome"].ToString() : nome);
                    updateCommand.Parameters.AddWithValue("@contato", string.IsNullOrEmpty(contato) ? reader["contato"].ToString() : contato);
                    updateCommand.Parameters.AddWithValue("@id", id);

                    updateCommand.ExecuteNonQuery();
                    Console.WriteLine("Fornecedor atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Fornecedor não encontrado.");
                }
            }
        }

        private void RemoverFornecedor()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|       Remover Fornecedor          |");
            Console.WriteLine("+-----------------------------------+");
            Console.Write("ID do Fornecedor: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Tem certeza que deseja remover o fornecedor? (S/N): ");
            string confirmacao = Console.ReadLine();

            if (confirmacao.ToUpper() != "S")
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Fornecedores WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Fornecedor removido com sucesso!");
                }
                else
                {
                    Console.WriteLine("Fornecedor não encontrado.");
                }
            }
        }
    }
}
