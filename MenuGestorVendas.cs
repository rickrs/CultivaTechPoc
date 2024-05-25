using System;

namespace CultivaTechPoc
{
    internal class MenuGestorVendas
    {
        private readonly GerenciamentoProducao _gerenciamentoProducao;

        public MenuGestorVendas()
        {
            _gerenciamentoProducao = new GerenciamentoProducao();
        }

        public void Exibir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("|       Menu do Gestor de Venda     |");
                Console.WriteLine("+-----------------------------------+");
                Console.WriteLine("| 1 - Simulação de Vendas           |");
                Console.WriteLine("| 2 - Listar Plantações em Andamento|");
                Console.WriteLine("| 3 - Listar Plantações Finalizadas |");
                Console.WriteLine("| 4 - Relatório de Produção Total   |");
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
                        SimulacaoVendas();
                        break;
                    case "2":
                        _gerenciamentoProducao.ListarPlantacoesEmAndamento();
                        break;
                    case "3":
                        _gerenciamentoProducao.ListarPlantacoesFinalizadas();
                        break;
                    case "4":
                        RelatorioProducaoTotal();
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
    }
}
