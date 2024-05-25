using System;

namespace CultivaTechPoc
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Login login = new Login();
                Usuarios usuario = login.Entrar();

                if (usuario != null)
                {
                    RedirecionarParaMenu(usuario);
                }
                else
                {
                    break;
                }
            }
        }

        private static void RedirecionarParaMenu(Usuarios usuario)
        {
            if (usuario.Tipo == "Admin")
            {
                MenuAdministrador menuAdmin = new MenuAdministrador();
                menuAdmin.Exibir();
            }
            else if (usuario.Tipo == "Gestor")
            {
                MenuGestorVendas menuGestor = new MenuGestorVendas();
                menuGestor.Exibir();
            }
            else
            {
                Console.WriteLine("Tipo de usuário desconhecido. Pressione qualquer tecla para sair.");
                Console.ReadKey();
            }
        }
    }
}
