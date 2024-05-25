using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CultivaTechPoc
{
    internal class RelatorioProducao
    {
        private readonly string _connectionString;

        public RelatorioProducao()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("CultivaTechDB");
        }

        public void ExibirRelatorio()
        {
            Console.Clear();
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("|      Relatório de Produção        |");
            Console.WriteLine("+-----------------------------------+");

            var statusPlantacoes = ObterStatusPlantacoes();
            var insumosUtilizados = ObterQuantidadeInsumosUtilizados();

            Console.WriteLine("Status das Plantações:");
            foreach (var status in statusPlantacoes)
            {
                Console.WriteLine($"- {status.Key}: {status.Value}");
            }

            Console.WriteLine("\nQuantidade de Insumos Utilizados:");
            foreach (var insumo in insumosUtilizados)
            {
                Console.WriteLine($"- {insumo.Key}: {insumo.Value}");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar.");
            Console.ReadKey();
        }

        private Dictionary<string, int> ObterStatusPlantacoes()
        {
            var statusPlantacoes = new Dictionary<string, int>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT status, COUNT(*) AS quantidade FROM Plantacoes GROUP BY status";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string status = reader["status"].ToString();
                    int quantidade = Convert.ToInt32(reader["quantidade"]);
                    statusPlantacoes[status] = quantidade;
                }
            }

            return statusPlantacoes;
        }

        private Dictionary<string, int> ObterQuantidadeInsumosUtilizados()
        {
            var insumosUtilizados = new Dictionary<string, int>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        i.nome AS insumo_nome, 
                        SUM(pi.quantidade) AS total_utilizado 
                    FROM 
                        PlantacaoInsumos pi
                    JOIN 
                        Insumos i ON pi.insumo_id = i.id
                    GROUP BY 
                        i.nome";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string insumoNome = reader["insumo_nome"].ToString();
                    int totalUtilizado = Convert.ToInt32(reader["total_utilizado"]);
                    insumosUtilizados[insumoNome] = totalUtilizado;
                }
            }

            return insumosUtilizados;
        }
    }
}
