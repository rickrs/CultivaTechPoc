using System;
using Microsoft.Data.SqlClient;

namespace CultivaTechPoc
{
    internal class BancoDeDados
    {
        private readonly string _connectionString;

        public BancoDeDados(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Usuarios ValidarCredenciais(string email, string senha)
        {
            string query = "SELECT id, nome, email, tipo FROM Usuarios WHERE email = @Email AND senha = @Senha";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Senha", senha);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuarios
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Email = reader.GetString(2),
                                Tipo = reader.GetString(3)
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao conectar ao banco de dados: " + ex.Message);
                }
            }

            return null;
        }
    }
}
