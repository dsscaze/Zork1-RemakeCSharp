using System;

namespace ZorkBrasil.Core.Enums
{
    [Flags]
    public enum FlagsSala
    {
        Nenhuma = 0,
        
        // ZIL: LIGHTBIT (Se a sala tem luz natural)
        Iluminada = 1, 
        
        // ZIL: TOUCHBIT (Se o jogador já esteve aqui - útil para descrição curta/longa)
        Visitada = 2,  
        
        // ZIL: ON-LAND (Terra firme) vs ON-WATER (Precisa de barco)
        Aquatica = 4,  
        
        // ZIL: INDOORS (Dentro de casa/caverna)
        Interior = 8,  
        
        // Para o futuro Top-Down: Bloqueia visão ou movimento especial
        Labirinto = 16,
        Morte = 32 // Salas armadilha
    }
}