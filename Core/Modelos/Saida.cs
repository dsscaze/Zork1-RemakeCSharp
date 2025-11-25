namespace ZorkBrasil.Core.Modelos
{
    public class Saida
    {
        public string SalaDestinoId { get; set; }
        
        // Se true, o jogador não passa.
        // No futuro (Top-Down), isso desenha uma porta fechada.
        public bool EstaBloqueada { get; set; } 
        
        // Se bloqueada, qual item abre? (Ex: "chave_esqueleto")
        public string IdChaveNecessaria { get; set; } 
        
        // Mensagem se tentar passar bloqueado (Ex: "A porta está pregada.")
        public string? MensagemBloqueio { get; set; }
        
        // Se é uma porta visível ou passagem secreta
        public bool ESecreta { get; set; }

        public Saida(string destinoId, bool bloqueada = false)
        {
            SalaDestinoId = destinoId;
            EstaBloqueada = bloqueada;
        }
    }
}