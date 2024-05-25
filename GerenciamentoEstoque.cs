using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace CultivaTechPoc
{
    internal class GerenciamentoEstoque
    {
        private readonly string _connectionString;

        public GerenciamentoEstoque()
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
                Console.WriteLine("|    Gerenciamento de Estoque      |");
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("| 1 - Adicionar Insumo             |");
                Console.WriteLine("| 2 - Listar Insumos               |");
                Console.WriteLine("| 3 - Editar Insumo                |");
                Console.WriteLine("| 4 - Remover Insumo               |");
                Console.WriteLine("| 5 - Adicionar Produto            |");
                Console.WriteLine("| 0 - Voltar ao Menu Principal     |");
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
                        AdicionarInsumo();
                        break;
                    case "2":
                        ListarInsumos();
                        break;
                    case "3":
                        EditarInsumo();
                        break;
                    case "4":
                        RemoverInsumo();
                        break;
                    case "5":
                        AdicionarProduto();
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

        private void AdicionarInsumo()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|        Adicionar Insumo           |");
            Console.WriteLine("+-----------------------------------+");

            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Quantidade: ");
            int quantidade = int.Parse(Console.ReadLine());
            Console.Write("Data de Validade (AAAA-MM-DD): ");
            DateTime dataValidade = DateTime.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Insumos (nome, quantidade, dataValidade) VALUES (@nome, @quantidade, @dataValidade)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@quantidade", quantidade);
                command.Parameters.AddWithValue("@dataValidade", dataValidade);

                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Insumo adicionado com sucesso!");
            }
        }

        private void ListarInsumos()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|          Listar Insumos           |");
            Console.WriteLine("+-----------------------------------+");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Insumos";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}");
                    Console.WriteLine($"Nome: {reader["nome"]}");
                    Console.WriteLine($"Quantidade: {reader["quantidade"]}");
                    Console.WriteLine($"Data de Validade: {reader["dataValidade"]}");
                    Console.WriteLine("+-----------------------------------+");
                }
            }
        }

        private void EditarInsumo()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|          Editar Insumo            |");
            Console.WriteLine("+-----------------------------------+");

            Console.Write("ID do Insumo: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string selectQuery = "SELECT * FROM Insumos WHERE id = @id";
                SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();

                if (reader.Read())
                {
                    Console.Write($"Nome ({reader["nome"]}): ");
                    string nome = Console.ReadLine();
                    Console.Write($"Quantidade ({reader["quantidade"]}): ");
                    string quantidadeInput = Console.ReadLine();
                    int quantidade = string.IsNullOrEmpty(quantidadeInput) ? (int)reader["quantidade"] : int.Parse(quantidadeInput);
                    Console.Write($"Data de Validade ({reader["dataValidade"]}): ");
                    string dataValidadeInput = Console.ReadLine();
                    DateTime dataValidade = string.IsNullOrEmpty(dataValidadeInput) ? (DateTime)reader["dataValidade"] : DateTime.Parse(dataValidadeInput);

                    reader.Close();

                    string updateQuery = "UPDATE Insumos SET nome = @nome, quantidade = @quantidade, dataValidade = @dataValidade WHERE id = @id";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@nome", nome);
                    updateCommand.Parameters.AddWithValue("@quantidade", quantidade);
                    updateCommand.Parameters.AddWithValue("@dataValidade", dataValidade);
                    updateCommand.Parameters.AddWithValue("@id", id);

                    updateCommand.ExecuteNonQuery();
                    Console.WriteLine("Insumo atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Insumo não encontrado.");
                }
            }
        }

        private void RemoverInsumo()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|          Remover Insumo           |");
            Console.WriteLine("+-----------------------------------+");

            Console.Write("ID do Insumo: ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Insumos WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Insumo removido com sucesso!");
                }
                else
                {
                    Console.WriteLine("Insumo não encontrado.");
                }
            }
        }

        private void AdicionarProduto()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|         Adicionar Produto         |");
            Console.WriteLine("+-----------------------------------+");

            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Preço: ");
            decimal preco = decimal.Parse(Console.ReadLine());
            Console.Write("Quantidade em Estoque: ");
            int quantidade = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Produtos (nome, preco, quantidadeEmEstoque) VALUES (@nome, @preco, @quantidadeEmEstoque)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@preco", preco);
                command.Parameters.AddWithValue("@quantidadeEmEstoque", quantidade);

                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Produto adicionado com sucesso!");
            }
        }
    }
}
