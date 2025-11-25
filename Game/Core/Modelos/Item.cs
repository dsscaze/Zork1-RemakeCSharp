using System.Collections.Generic;
using ZorkBrasil.Core.Enums;

namespace ZorkBrasil.Core.Modelos
{
    public class Item
    {
        // ID único no código (ex: "brass_lantern")
        public string Id { get; private set; }

        // Nome principal exibido na lista (ex: "Lanterna de latão")
        public string Nome { get; set; }

        // Sinônimos aceitos no parser (ex: "lanterna", "luz", "lampada")
        public List<string> Sinonimos { get; set; }

        // Descrição ao examinar ("olhar lanterna")
        public string Descricao { get; set; }

        // Texto exibido quando o item está no chão da sala antes de ser pego
        // Ex: "Há uma lanterna brilhante aqui."
        public string DescricaoNoChao { get; set; }

        // Comportamentos (Bits)
        public FlagsItem Flags { get; set; }

        // Se for um Container (ex: Caixa de Correio), guarda itens dentro
        public List<Item> Conteudo { get; set; }

        // -------------------------------------------------------
        // FUTURO TOP-DOWN
        // Nome do arquivo de sprite/ícone (ex: "itens/lantern_on.png")
        public string NomeSprite { get; set; }
        // -------------------------------------------------------

        public Item(string id, string nome, string descricao, FlagsItem flagsIniciais = FlagsItem.Pegavel)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            DescricaoNoChao = descricao; // Padrão igual, pode mudar depois
            Flags = flagsIniciais;
            Sinonimos = new List<string>();
            Conteudo = new List<Item>();
            NomeSprite = "default_item";
        }

        // Métodos auxiliares para verificar flags rapidamente
        public bool EhPegavel() => Flags.HasFlag(FlagsItem.Pegavel);
        public bool EhContainer() => Flags.HasFlag(FlagsItem.Container);
        
        public bool EstaAberto() => Flags.HasFlag(FlagsItem.EstaAberto);
        
        public bool EmiteLuz() 
        {
            // Um item emite luz se for FonteDeLuz E estiver Aceso
            // OU se for algo que brilha sempre (ex: pedra mágica - adaptável)
            return Flags.HasFlag(FlagsItem.FonteDeLuz) && Flags.HasFlag(FlagsItem.EstaAceso);
        }

        public void AdicionarSinonimo(params string[] palavras)
        {
            foreach(var p in palavras)
                Sinonimos.Add(p.ToLower());
        }

        public string Localizacao(Jogador jogador, Sala sala)
        {
            if (jogador.Inventario.Contains(this))
                return "inventario";
            if (sala.Itens.Contains(this))
                return "sala";
            return "desconhecido";
        }
    }
}