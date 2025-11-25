using System;
using ZorkBrasil.Core.Logica;

class Program
{
    static void Main(string[] args)
    {
        // Configuração inicial
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        
        var motor = new Motor();

        // Conecta o texto do jogo ao Console
        motor.OnMensagem += (texto) => 
        {
            Console.WriteLine(texto);
        };

        // Inicia o jogo
        motor.IniciarJogo();

        bool rodando = true;
        while (rodando)
        {
            // --- AQUI ESTÁ A MÁGICA ---
            // Atualiza a barra de título da janela do Windows/Terminal
            AtualizarTitulo(motor);
            // --------------------------

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n> ");
            Console.ResetColor();

            string? comando = Console.ReadLine();

            if (string.IsNullOrEmpty(comando)) continue;

            if (comando.ToLower() == "sair" || comando.ToLower() == "quit")
            {
                rodando = false;
            }
            else
            {
                motor.ProcessarComando(comando);
            }
        }
    }

    static void AtualizarTitulo(Motor motor)
    {
        // Pega os dados do motor sem interferir na lógica
        string local = motor.Mundo[motor.Jogador.SalaAtualId].Nome;
        int pontos = motor.Jogador.Pontuacao;
        int turnos = motor.Jogador.Turnos;

        // Formata o texto da barra de título
        // Exemplo: Zork Brasil | Oeste da Casa | Pontos: 0 | Turnos: 5
        Console.Title = $"Zork Brasil | {local} | Pontos: {pontos} | Turnos: {turnos}";
    }
}