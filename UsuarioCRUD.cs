using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace CultivaTechPoc
{
    internal class UsuarioCRUD
    {
        private readonly string _connectionString;

        public UsuarioCRUD(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AdicionarUsuario(Usuarios usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Usuarios (Nome, Email, Senha, Tipo) VALUES (@Nome, @Email, @Senha, @Tipo)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Senha", usuario.Senha);
                command.Parameters.AddWithValue("@Tipo", usuario.Tipo);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Usuarios> ListarUsuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Nome, Email, Tipo FROM Usuarios";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Usuarios usuario = new Usuarios
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2),
                        Tipo = reader.GetString(3)
                    };
                    usuarios.Add(usuario);
                }
            }

            return usuarios;
        }

        public void AtualizarUsuario(Usuarios usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Tipo = @Tipo WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", usuario.Id);
                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Tipo", usuario.Tipo);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RemoverUsuario(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Usuarios WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
