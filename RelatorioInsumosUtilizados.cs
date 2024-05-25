using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CultivaTechPoc
{
    internal class RelatorioInsumosUtilizados
    {
        private readonly string _connectionString;

        public RelatorioInsumosUtilizados()
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
            Console.WriteLine("|  Relatório de Insumos Utilizados  |");
            Console.WriteLine("+-----------------------------------+");

            var insumosUtilizados = ObterQuantidadeInsumosUtilizados();

            Console.WriteLine("Quantidade de Insumos Utilizados:");
            foreach (var insumo in insumosUtilizados)
            {
                Console.WriteLine($"- {insumo.Key}: {insumo.Value}");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar.");
            Console.ReadKey();
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
