using System.Collections.Generic;
using System.Linq;

namespace ZorkBrasil.Core.Modelos
{
    public class Jogador
    {
        // Onde o jogador está (ID da Sala)
        public string SalaAtualId { get; set; }

        // O que ele carrega
        public List<Item> Inventario { get; set; }

        // Estatísticas do Zork
        public int Pontuacao { get; set; }
        public int Turnos { get; set; }
        
        // Limite de peso ou quantidade de itens (opcional, mas Zork tem)
        public int CapacidadeMaxima { get; set; } = 7; 

        public Jogador(string salaInicialId)
        {
            SalaAtualId = salaInicialId;
            Inventario = new List<Item>();
            Pontuacao = 0;
            Turnos = 0;
        }

        // Verifica se tem o item (pelo ID ou Nome)
        public bool TemItem(string nomeOuId)
        {
            return Inventario.Any(i => 
                i.Id == nomeOuId || 
                i.Nome.ToLower().Contains(nomeOuId.ToLower()) ||
                i.Sinonimos.Contains(nomeOuId.ToLower())
            );
        }

        // Adiciona item ao inventário
        public bool AdicionarItem(Item item)
        {
            if (Inventario.Count >= CapacidadeMaxima)
                return false; // Inventário cheio

            Inventario.Add(item);
            return true;
        }

        // Remove item
        public Item? RemoverItem(string nomeOuId)
        {
            var item = Inventario.FirstOrDefault(i => 
                i.Id == nomeOuId || 
                i.Nome.ToLower() == nomeOuId.ToLower() || 
                i.Sinonimos.Contains(nomeOuId.ToLower())
            );

            if (item != null)
            {
                Inventario.Remove(item);
            }
            return item;
        }
    }
}