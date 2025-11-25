using System;
using System.Collections.Generic;
using System.Text;
using ZorkBrasil.Core.Enums;

namespace ZorkBrasil.Core.Modelos
{
    public class Sala
    {
        // ID único (ex: "west_house", "living_room") - Igual ao ZIL
        public string Id { get; private set; }

        // Nome exibido na interface/topo da tela
        public string Nome { get; set; }

        // Descrição completa (quando entra pela primeira vez ou usa "olhar")
        public string DescricaoLonga { get; set; }

        // Descrição breve (quando volta na sala). Se null, usa a Longa.
        public string DescricaoCurta { get; set; }

        // Propriedades de estado (Luz, Água, Visitada)
        public FlagsSala Flags { get; set; }

        // ---------------------------------------------------------
        // PREPARAÇÃO PARA TOP-DOWN (FUTURO)
        // Coordenadas no Grid do Mundo (X, Y, Andar). 
        // No console não usamos, mas no Unity isso definirá a posição do Prefab.
        public int PosicaoX { get; set; }
        public int PosicaoY { get; set; }
        public int AndarZ { get; set; } // 0 = Térreo, -1 = Porão
        // ---------------------------------------------------------

        // Inventário da Sala (Itens no chão)
        // Assumindo que você criou a classe Item (posso fornecer se precisar)
        public List<Item> Itens { get; set; }

        // As conexões para outras salas
        public Dictionary<Direcao, Saida> Saidas { get; set; }

        // EVENTOS / DELEGATES
        // Zork tem lógicas únicas por sala (ex: Sala do Eco, Sala que gira).
        // Essa Action permite injetar código customizado sem sujar a classe base.
        // Recebe o input do jogador para tratar casos especiais.
        public Func<string, string> AcaoEspecialDaSala { get; set; }

        // Construtor
        public Sala(string id, string nome, string descricaoLonga, int x = 0, int y = 0, int z = 0)
        {
            Id = id;
            Nome = nome;
            DescricaoLonga = descricaoLonga;
            PosicaoX = x;
            PosicaoY = y;
            AndarZ = z;

            Itens = new List<Item>();
            Saidas = new Dictionary<Direcao, Saida>();
            Flags = FlagsSala.Nenhuma;
        }

        // Método auxiliar para obter descrição baseado se já visitou
        public string ObterDescricao()
        {
            // Se tem luz (natural ou artificial flag), mostra texto. Senão, breu.
            // Nota: A lógica de verificar lanterna do jogador fica no Motor, aqui verificamos a sala.
            if (!Flags.HasFlag(FlagsSala.Iluminada))
            {
                // O Motor deve checar se o jogador tem lanterna antes de chamar isso,
                // ou retornamos um status indicando escuridão.
                return "Está breu. Você pode ser devorado por um Grue.";
            }

            // Se já visitou e tem descrição curta, usa ela (padrão Zork "Superbrief" ou "Brief")
            if (Flags.HasFlag(FlagsSala.Visitada) && !string.IsNullOrEmpty(DescricaoCurta))
            {
                return DescricaoCurta;
            }

            // Marca como visitada automaticamente ao ler a descrição completa
            Flags |= FlagsSala.Visitada;
            return DescricaoLonga;
        }

        // Adiciona conexão facilmente
        public void DefinirSaida(Direcao direcao, string idDestino, bool bloqueada = false, string? msgBloqueio = null)
        {
            var saida = new Saida(idDestino, bloqueada)
            {
                MensagemBloqueio = msgBloqueio
            };
            
            if (Saidas.ContainsKey(direcao))
                Saidas[direcao] = saida;
            else
                Saidas.Add(direcao, saida);
        }

        // Método para desenhar no console (Debug/Protótipo do TopDown)
        public override string ToString()
        {
            return $"[{Id}] {Nome} (X:{PosicaoX}, Y:{PosicaoY}) - Itens: {Itens.Count}";
        }
    }
}