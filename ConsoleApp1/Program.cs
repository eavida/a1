using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices.Marshalling;

public class Program
{
    // Definindo os valores das cartas
    public static Dictionary<string, int> cartaValores = new Dictionary<string, int>()
    {
        {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6}, {"7", 7}, {"8", 8}, {"9", 9}, {"10", 10},
        {"J", 10}, {"Q", 10}, {"K", 10}, {"A", 11}
    };
    //Declarando Valores
     const double dinheiroinicial = 200;
    static double dinheiroJogador = dinheiroinicial;
    static int Apostado = 0;
    static int Vitorias = 0;
    static bool IsGameRunning = true;
    public static Random random = new Random();
    // Inicio
    public static void Main(string[] args)
    { 
        Console.Title = "BlackJack/21";
        
        while(IsGameRunning){
           PrintStart();
            Thread.Sleep(1000);
            Console.Clear();
        }

    
    }
    //Menu Inicial
    public static void PrintStart(){
        Console.WriteLine("     ***** **   ***                             *                                       *        ");
        Console.WriteLine("  ******  ***    ***                          **           *                          **         ");
        Console.WriteLine(" **   *  * **     **                          **          ***                         **         ");
        Console.WriteLine("*    *  *  **     **                          **           *                          **         ");
        Console.WriteLine("    *  *   *      **                          **                                      **         ");
        Console.WriteLine("   ** **  *       **       ****       ****    **  ***    ***       ****       ****    **  ***    ");
        Console.WriteLine("   ** ** *        **      * ***  *   * ***  * ** * ***    ***     * ***  *   * ***  * ** * ***   ");
        Console.WriteLine("   ** ***         **     *   ****   *   ****  ***   *      **    *   ****   *   ****  ***   *    ");
        Console.WriteLine("   ** ** ***      **    **    **   **         **   *       *    **    **   **         **   *     ");
        Console.WriteLine("   ** **   ***    **    **    **   **         **  *       *     **    **   **         **  *      ");
        Console.WriteLine("   *  **     **   **    **    **   **         ** **      ***    **    **   **         ** **      ");
        Console.WriteLine("      *      **   **    **    **   **         ******      ***   **    **   **         ******     ");
        Console.WriteLine("  ****     ***    **    **    **   ***     *  **  ***      ***  **    **   ***     *  **  ***    ");
        Console.WriteLine(" *  ********      *** *  ***** **   *******   **   *** *    ***  ***** **   *******   **   *** * ");
        Console.WriteLine("*     ****         ***    ***   **   *****     **   ***      **   ***   **   *****     **   ***  ");
        Console.WriteLine("*                                                            **                                  ");
        Console.WriteLine(" **                                                          *                                   ");
        Console.WriteLine("                                                            *                                    ");
        Console.WriteLine("                                                           *                                     ");
        Console.WriteLine("                                                                                                 ");
        Console.WriteLine("Bem-vindo ao Blackjack!");
        Console.WriteLine("Voce consegue chegar a 25 vitorias seguidas botando tudo na mesa?");
        Console.WriteLine("Vitorias seguidas:"+ Vitorias);
        Console.WriteLine("Jogar[1] ou Sair[2]?");
        string escolha = Console.ReadLine();
        if (escolha == "1"){
            Jogo();
        }
        else if (escolha == "2"){
            Console.Clear();
            System.Environment.Exit(1);
        }
        else {
            Console.WriteLine("Invalido");
        }
    }

    // Função principal do jogo
    private static void Jogo(){

        // Inicializando o baralho
        List<string> baralho = CriarBaralho();
        List<string> maoJogador = new List<string>();
        List<string> maoComputador = new List<string>();


        // Distribuindo as cartas iniciais
        maoJogador.Add(PegarCarta(baralho));
        maoJogador.Add(PegarCarta(baralho));
        maoComputador.Add(PegarCarta(baralho));
        maoComputador.Add(PegarCarta(baralho));

        //Aposta
        Apostando();
        Thread.Sleep(1000);

        // Exibindo cartas do jogador e do computador
        Console.WriteLine("\nCarta visível do computador:");
        ExibirCartaASCII(maoComputador[0], "Computador");

        Console.WriteLine("Total do jogador: " + CalcularPontos(maoJogador));
        ExibirCartasASCII(maoJogador, "Jogador");
        //Total de cada
        Console.WriteLine("Total da carta visível do computador: " + cartaValores[maoComputador[0].Split(' ')[0]]);

        // Turno do jogador
        bool jogadorContinua = true;
        while (jogadorContinua && CalcularPontos(maoJogador) < 21)
        {
            Console.WriteLine("Você tem " + CalcularPontos(maoJogador) + " pontos.");
            Console.WriteLine("Deseja (1) pedir mais uma carta ou (2) parar?");
            string escolha = Console.ReadLine();

            if (escolha == "1")
            {
                maoJogador.Add(PegarCarta(baralho));
                ExibirCartasASCII(maoJogador, "Jogador");
                Console.WriteLine("Cartas do jogador: " + string.Join(", ", maoJogador) + " - Total: " + CalcularPontos(maoJogador));
            }
            else if (escolha == "2")
            {
                jogadorContinua = false;
            }
            else
            {
                Console.WriteLine("Escolha inválida.");
            }
        }

        // Se o jogador ultrapassou 21, ele perdeu automaticamente
        if (CalcularPontos(maoJogador) > 21)
        {
            Console.WriteLine("Você perdeu! Sua pontuação ultrapassou 21.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The House Always Wins");
            Console.ForegroundColor = ConsoleColor.Gray;
            Vitorias = 0;
            Thread.Sleep(5000);
            return;
        }
        //Ver se ele bateu 21
        if (CalcularPontos(maoJogador) == 21)
        {
            Console.WriteLine("Você Ganhou!");
            dinheiroJogador = dinheiroJogador + (Apostado*2);
            Vitorias++;
            Thread.Sleep(5000);
            return;
        }

        // Turno do computador
        Console.WriteLine("\nTurno do computador:");
        ExibirCartasASCII(maoComputador, "Computador");
        Console.WriteLine("Cartas do computador: " + string.Join(", ", maoComputador) + " - Total: " + CalcularPontos(maoComputador));

        while (CalcularPontos(maoComputador) < 17)
        {
            maoComputador.Add(PegarCarta(baralho));
            ExibirCartasASCII(maoComputador, "Computador");
            Console.WriteLine("Cartas do computador: " + string.Join(", ", maoComputador) + " - Total: " + CalcularPontos(maoComputador));
        }

        // Resultado final
        int pontosJogador = CalcularPontos(maoJogador);
        int pontosComputador = CalcularPontos(maoComputador);
        //Definir Vencedor
        if (pontosComputador > 21)
        {
            Console.WriteLine("O computador estourou! Você venceu.");
            dinheiroJogador = dinheiroJogador + (Apostado*2);
            Vitorias++;
            Thread.Sleep(5000);
            
        }
        else if (pontosJogador > pontosComputador)
        {
            Console.WriteLine("Você venceu! Sua pontuação: " + pontosJogador + " - Pontuação do computador: " + pontosComputador);
            dinheiroJogador = dinheiroJogador + (Apostado*2);
            Vitorias++;
            Thread.Sleep(5000);
        }
        else if (pontosJogador < pontosComputador)
        {
            Console.WriteLine("O computador venceu! Sua pontuação: " + pontosJogador + " - Pontuação do computador: " + pontosComputador);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The House Always Wins");
            Console.ForegroundColor = ConsoleColor.Gray;
            dinheiroJogador = dinheiroJogador - Apostado;
            Vitorias = 0;
            Thread.Sleep(5000);
        }
        else
        {
            Console.WriteLine("Empate! Sua pontuação: " + pontosJogador + " - Pontuação do computador: " + pontosComputador);
        }
        
    } 

    // Função para criar um baralho
    public static List<string> CriarBaralho()
    {
        List<string> baralho = new List<string>();
        string[] naipes = { "♥", "♦", "♣", "♠" };
        string[] valores = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        foreach (var naipe in naipes)
        {
            foreach (var valor in valores)
            {
                baralho.Add(valor + " de " + naipe);
            }
        }
        return baralho;
    }

    // Função para pegar uma carta aleatória do baralho
    public static string PegarCarta(List<string> baralho)
    {
        int index = random.Next(baralho.Count);
        string carta = baralho[index];
        baralho.RemoveAt(index);
        return carta;
    }

    // Função para calcular os pontos de uma mão
    public static int CalcularPontos(List<string> mao)
    {
        int pontos = 0;
        int ases = 0;

        foreach (var carta in mao)
        {
            string valor = carta.Split(' ')[0]; // Extrai o valor da carta
            pontos += cartaValores[valor];

            if (valor == "A")
                ases++;
        }

        // Ajustando os ases (de 11 para 1 se necessário)
        while (pontos > 21 && ases > 0)
        {
            pontos -= 10;
            ases--;
        }

        return pontos;
    }
    
//Sistema de Aposta
    private static void Apostando()
    {
        Console.WriteLine("Dinheiro Disponivel:"+ dinheiroJogador);
        if (dinheiroJogador<= 0 ){
            Console.WriteLine("Sem Dinheiro! Va embora");
            Thread.Sleep(4000);
            System.Environment.Exit(1);
            
        }
        Console.WriteLine("Gostaria de Apostar Quanto?");
        Apostado = int.Parse(Console.ReadLine());
        Console.WriteLine("Aposta = "+ Apostado);
        ApostaCheck();
        if (!ApostaCheck())
            {
                ApostaInvalida();
            }
    }
//Verifica a Aposta
    private static bool ApostaCheck()
        {
            return Apostado <= dinheiroJogador;
        }
    //Reclama da Aposta
    private static void ApostaInvalida(){
        Console.WriteLine("Sem Dinheiro Suficiente");
        Thread.Sleep(4000);
        Apostando();
    }

    // Função para exibir uma carta em ASCII
    public static void ExibirCartaASCII(string carta, string dono)
    {
        string valor = carta.Split(' ')[0];
        string naipe = carta.Split(' ')[2];

        Console.WriteLine($"+-------+");
        Console.WriteLine($"| {valor.PadLeft(2)} {naipe} |");
        Console.WriteLine($"|       |");
        Console.WriteLine($"|   {valor.PadLeft(2)}   |");
        Console.WriteLine($"|       |");
        Console.WriteLine($"|       |");
        Console.WriteLine($"| {naipe}     |");
        Console.WriteLine($"+-------+");
    }

    // Função para exibir as cartas do jogador ou computador em ASCII
    public static void ExibirCartasASCII(List<string> mao, string dono)
    {
        foreach (var carta in mao)
        {
            ExibirCartaASCII(carta, dono);
            Console.WriteLine(); // Espaço entre as cartas
        }
    }


}
