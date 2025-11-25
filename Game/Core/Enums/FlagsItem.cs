using System;

namespace ZorkBrasil.Core.Enums
{
    [Flags]
    public enum FlagsItem
    {
        Nenhuma = 0,
        
        // ZIL: TAKEBIT (Pode colocar no inventário)
        Pegavel = 1,
        
        // ZIL: CONTBIT (É um recipiente, ex: Saco, Caixa de Correio)
        Container = 2,
        
        // ZIL: OPENBIT (Pode abrir/fechar)
        PodeAbrir = 4,
        
        // Estado atual: Está aberto?
        EstaAberto = 8,
        
        // ZIL: LIGHTBIT (Emite luz se estiver aceso)
        FonteDeLuz = 16,
        
        // ZIL: ONBIT (Se a lanterna está ligada)
        EstaAceso = 32,
        
        // ZIL: READBIT (Pode ser lido)
        Legivel = 64,
        
        // ZIL: WEAPONBIT (Pode ser usado para atacar)
        Arma = 128,

        // ZIL: TOOLBIT (Ferramenta que pode ser usada para abrir/manipular objetos)
        Ferramenta = 256,

        // ZIL: DRINKBIT / EATBIT
        Comestivel = 512,
        
        // Para o futuro Top-Down: Item fixo no cenário (ex: Móvel pesado)
        Cenario = 1024
    }
}