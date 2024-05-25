using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace CultivaTechPoc
{
    internal class GerenciamentoProducao
    {
        private List<Producao> producoes;
        private readonly string _connectionString;

        public GerenciamentoProducao()
        {
            // Carrega os dados do JSON ao iniciar a classe
            producoes = CarregarProducoes();

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
                Console.WriteLine("+-------------------------------------+");
                Console.WriteLine("|       Gerenciamento de Produção     |");
                Console.WriteLine("+-------------------------------------+");
                Console.WriteLine("| 1 - Nova Produção                   |");
                Console.WriteLine("| 2 - Ativar Plantação                |");
                Console.WriteLine("| 3 - Listar Plantações em Andamento  |");
                Console.WriteLine("| 4 - Listar Plantações Aguardando Insumos |");
                Console.WriteLine("| 5 - Listar Plantações Finalizadas   |");
                Console.WriteLine("| 0 - Voltar ao Menu Principal        |");
                Console.WriteLine("+-------------------------------------+");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine();

                if (opcao == "0")
                {
                    break;
                }

                switch (opcao)
                {
                    case "1":
                        NovaProducao();
                        break;
                    case "2":
                        AtivarPlantacao();
                        break;
                    case "3":
                        ListarPlantacoesEmAndamento();
                        break;
                    case "4":
                        ListarPlantacoesAguardandoInsumos();
                        break;
                    case "5":
                        ListarPlantacoesFinalizadas();
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

        private List<Producao> CarregarProducoes()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Iafictiicia.json");
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Producao>>(json);
        }

        private void NovaProducao()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|          Nova Produção           |");
            Console.WriteLine("+-----------------------------------+");
            Console.Write("Digite sua Localização: ");
            string localizacao = Console.ReadLine();
            Console.WriteLine("|                                   |");
            Console.Write("Digite o Mês Atual (ex: Janeiro): ");
            string mesAtual = Console.ReadLine();
            Console.WriteLine("|                                   |");
            Console.WriteLine("| [1] Gerar Recomendação           |");
            Console.WriteLine("| [0] Voltar ao Menu Principal      |");
            Console.WriteLine("+-----------------------------------+");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();

            if (opcao == "0")
            {
                return;
            }
            else if (opcao == "1")
            {
                var recomendacao = producoes.Find(p => p.Cidade.Equals(localizacao, StringComparison.OrdinalIgnoreCase) && p.MesIdeal.Equals(mesAtual, StringComparison.OrdinalIgnoreCase));
                if (recomendacao != null)
                {
                    ExibirRecomendacao(recomendacao, localizacao, mesAtual);
                }
                else
                {
                    Console.WriteLine("Nenhuma recomendação encontrada para a localização e mês fornecidos.");
                    Console.WriteLine("Pressione qualquer tecla para continuar.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                Console.ReadKey();
            }
        }

        private void ExibirRecomendacao(Producao recomendacao, string localizacao, string mesAtual)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("|          Nova Produção           |");
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine($"| Localização: {localizacao}");
                Console.WriteLine($"| Mês Atual: {mesAtual}");
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("| Recomendação:                     |");
                Console.WriteLine($"| - Tipo de Alimento: {recomendacao.Fruta}");
                Console.WriteLine("| - Insumos Necessários:            |");
                foreach (var insumo in recomendacao.InsumosNecessarios)
                {
                    Console.WriteLine($"|   - {insumo}");
                }
                Console.WriteLine("| - Requisitos de Cultivo:          |");
                Console.WriteLine($"|   - Cidade: {recomendacao.Cidade}");
                Console.WriteLine($"|   - Mês Ideal: {recomendacao.MesIdeal}");
                Console.WriteLine($"|   - Prazo de Colheita: {recomendacao.PrazoColheitaMeses} meses");
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("| [1] Aceitar                      |");
                Console.WriteLine("| [0] Recusar                      |");
                Console.WriteLine("+-----------------------------------+");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine();

                if (opcao == "0")
                {
                    return;
                }
                else if (opcao == "1")
                {
                    // Lógica para aceitar a recomendação e prosseguir com a produção
                    Console.WriteLine("Recomendação aceita. Iniciando produção...");
                    SalvarProducaoEPlantacao(localizacao, recomendacao);
                    Console.WriteLine("Produção e plantação cadastradas com sucesso!");
                    Console.WriteLine("Pressione qualquer tecla para continuar.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                }
            }
        }

        private void SalvarProducaoEPlantacao(string localizacao, Producao recomendacao)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Inserir produção
                    string insertProducaoQuery = "INSERT INTO Producoes (localizacao, dataCadastro, status) OUTPUT INSERTED.id VALUES (@localizacao, @dataCadastro, @status)";
                    SqlCommand insertProducaoCommand = new SqlCommand(insertProducaoQuery, connection, transaction);
                    insertProducaoCommand.Parameters.AddWithValue("@localizacao", localizacao);
                    insertProducaoCommand.Parameters.AddWithValue("@dataCadastro", DateTime.Now);
                    insertProducaoCommand.Parameters.AddWithValue("@status", "Iniciada");

                    int producaoId = (int)insertProducaoCommand.ExecuteScalar();

                    // Inserir plantação
                    string insertPlantacaoQuery = "INSERT INTO Plantacoes (alimento, dataCadastro, dataPlantio, dataColheita, status, producao_id) VALUES (@alimento, @dataCadastro, @dataPlantio, @dataColheita, @status, @producaoId)";
                    SqlCommand insertPlantacaoCommand = new SqlCommand(insertPlantacaoQuery, connection, transaction);
                    insertPlantacaoCommand.Parameters.AddWithValue("@alimento", recomendacao.Fruta);
                    insertPlantacaoCommand.Parameters.AddWithValue("@dataCadastro", DateTime.Now);
                    insertPlantacaoCommand.Parameters.AddWithValue("@dataPlantio", DateTime.Now);
                    insertPlantacaoCommand.Parameters.AddWithValue("@dataColheita", DateTime.Now.AddMonths(recomendacao.PrazoColheitaMeses));
                    insertPlantacaoCommand.Parameters.AddWithValue("@status", "Aguardando Insumos");
                    insertPlantacaoCommand.Parameters.AddWithValue("@producaoId", producaoId);

                    insertPlantacaoCommand.ExecuteNonQuery();

                    // Inserir insumos necessários para a plantação
                    foreach (var insumo in recomendacao.InsumosNecessarios)
                    {
                        string selectInsumoIdQuery = "SELECT id FROM Insumos WHERE nome = @insumoNome";
                        SqlCommand selectInsumoIdCommand = new SqlCommand(selectInsumoIdQuery, connection, transaction);
                        selectInsumoIdCommand.Parameters.AddWithValue("@insumoNome", insumo);
                        int insumoId = (int?)selectInsumoIdCommand.ExecuteScalar() ?? 0;

                        if (insumoId == 0)
                        {
                            throw new Exception($"Insumo '{insumo}' não encontrado no banco de dados.");
                        }

                        string insertPlantacaoInsumoQuery = "INSERT INTO PlantacaoInsumos (plantacao_id, insumo_id, quantidade) VALUES (@plantacaoId, @insumoId, @quantidade)";
                        SqlCommand insertPlantacaoInsumoCommand = new SqlCommand(insertPlantacaoInsumoQuery, connection, transaction);
                        insertPlantacaoInsumoCommand.Parameters.AddWithValue("@plantacaoId", producaoId);
                        insertPlantacaoInsumoCommand.Parameters.AddWithValue("@insumoId", insumoId);
                        insertPlantacaoInsumoCommand.Parameters.AddWithValue("@quantidade", 1); // Defina a quantidade conforme necessário

                        insertPlantacaoInsumoCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro ao salvar produção e plantação: " + ex.Message);
                }
            }
        }


        public void ListarPlantacoesEmAndamento()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|    Plantações em Andamento        |");
            Console.WriteLine("+-----------------------------------+");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Plantacoes WHERE status = 'Em Andamento'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}");
                    Console.WriteLine($"Alimento: {reader["alimento"]}");
                    Console.WriteLine($"Data de Cadastro: {reader["dataCadastro"]}");
                    Console.WriteLine($"Data de Plantio: {reader["dataPlantio"]}");
                    Console.WriteLine($"Data de Colheita: {reader["dataColheita"]}");
                    Console.WriteLine($"Status: {reader["status"]}");
                    Console.WriteLine("+-----------------------------------+");
                }
            }

            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();
        }

        public void ListarPlantacoesAguardandoInsumos()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|   Plantações Aguardando Insumos   |");
            Console.WriteLine("+-----------------------------------+");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Plantacoes WHERE status = 'Aguardando Insumos'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}");
                    Console.WriteLine($"Alimento: {reader["alimento"]}");
                    Console.WriteLine($"Data de Cadastro: {reader["dataCadastro"]}");
                    Console.WriteLine($"Insumos Faltantes: {ObterInsumosFaltantes(reader["id"].ToString())}");
                    Console.WriteLine("+-----------------------------------+");
                }
            }

            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();
        }

        private string ObterInsumosFaltantes(string plantacaoId)
        {
            List<string> insumosFaltantes = new List<string>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT nome FROM Insumos WHERE id NOT IN (SELECT insumo_id FROM PlantacaoInsumos WHERE plantacao_id = @plantacaoId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@plantacaoId", plantacaoId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    insumosFaltantes.Add(reader["nome"].ToString());
                }
            }

            return string.Join(", ", insumosFaltantes);
        }

        public void ListarPlantacoesFinalizadas()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|      Plantações Finalizadas       |");
            Console.WriteLine("+-----------------------------------+");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Plantacoes WHERE status = 'Finalizada'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}");
                    Console.WriteLine($"Alimento: {reader["alimento"]}");
                    Console.WriteLine($"Data de Cadastro: {reader["dataCadastro"]}");
                    Console.WriteLine($"Data de Plantio: {reader["dataPlantio"]}");
                    Console.WriteLine($"Data de Colheita: {reader["dataColheita"]}");
                    Console.WriteLine($"Status: {reader["status"]}");
                    Console.WriteLine("+-----------------------------------+");
                }
            }

            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();
        }

        private void AtivarPlantacao()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|         Ativar Plantação          |");
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("| Observação: Por se tratar de uma  |");
            Console.WriteLine("| Prova de Conceito (POC), todas as |");
            Console.WriteLine("| plantações podem ser ativadas. Em |");
            Console.WriteLine("| um ambiente de produção, somente  |");
            Console.WriteLine("| as plantações com insumos         |");
            Console.WriteLine("| necessários em estoque poderão    |");
            Console.WriteLine("| ser ativadas.                     |");
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("| ID | Alimento | Data Cadastro |   |");
            Console.WriteLine("|----|---------|--------------|---|");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Plantacoes WHERE status = 'Aguardando Insumos'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"| {reader["id"]} | {reader["alimento"]} | {reader["dataCadastro"]} |");
                }
            }

            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|                                   |");
            Console.WriteLine("| [ID] - Ativar                    |");
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
                int plantacaoId;
                if (int.TryParse(opcao, out plantacaoId))
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        string updateQuery = "UPDATE Plantacoes SET status = 'Em Andamento' WHERE id = @id";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@id", plantacaoId);

                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Plantação ativada com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Erro ao ativar plantação. Verifique o ID e tente novamente.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ID inválido. Pressione qualquer tecla para tentar novamente.");
                }
            }

            Console.WriteLine("Pressione qualquer tecla para continuar.");
            Console.ReadKey();
        }
    }
}
