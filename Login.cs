using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CultivaTechPoc
{
    internal class Login
    {
        private readonly BancoDeDados _bancoDeDados;

        public Login()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("CultivaTechDB");

            _bancoDeDados = new BancoDeDados(connectionString);
        }

        public Usuarios Entrar()
        {
            Usuarios usuarioLogado = null;

            while (usuarioLogado == null)
            {
                Console.Clear();
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("|             Login                |");
                Console.WriteLine("+-----------------------------------+");
                Console.Write(" Usuário: ");
                string usuario = Console.ReadLine();
                Console.WriteLine("|                                   |");
                Console.Write(" Senha: ");
                string senha = MascararSenha();
                Console.WriteLine("|                                   |");
                Console.WriteLine("| [1] Entrar                       |");
                Console.WriteLine("| [0] Sair                         |");
                Console.WriteLine("+-----------------------------------+");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    usuarioLogado = _bancoDeDados.ValidarCredenciais(usuario, senha);
                    if (usuarioLogado == null)
                    {
                        Console.WriteLine("Usuário ou senha incorretos. Pressione qualquer tecla para tentar novamente.");
                        Console.ReadKey();
                    }
                }
                else if (opcao == "0")
                {
                    Console.WriteLine("Saindo...");
                    break;
                }
                else
                {
                    Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                }
            }

            return usuarioLogado;
        }

        private string MascararSenha()
        {
            string senha = "";
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    Console.Write("\b \b");
                    senha = senha[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    senha += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return senha;
        }
    }
}
