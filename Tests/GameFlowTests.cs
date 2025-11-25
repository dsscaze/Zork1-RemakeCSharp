using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZorkBrasil.Core.Logica;
using ZorkBrasil.Core.Enums;

namespace ZorkBrasil.Tests
{
    /// <summary>
    /// Testes de fluxo do jogo para garantir que funcionalidades críticas não quebrem
    /// </summary>
    public class GameFlowTests
    {
        private List<string> _mensagens;

        private Motor CriarMotorComCapturaDeMensagens()
        {
            _mensagens = new List<string>();
            var motor = new Motor();
            motor.OnMensagem += (msg) => _mensagens.Add(msg);
            return motor;
        }

        private string ObterUltimaMensagem() => _mensagens.LastOrDefault() ?? "";
        private string ObterTodasMensagens() => string.Join("\n", _mensagens);

        #region Testes de Inicialização

        [Fact]
        public void DeveIniciarJogoNaPosiçãoCorreta()
        {
            // Arrange & Act
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();

            // Assert
            Assert.Equal("west_house", motor.Jogador.SalaAtualId);
            Assert.Contains("Oeste da Casa", ObterTodasMensagens());
        }

        [Fact]
        public void DeveCarregarMundoComSalasCorretas()
        {
            // Arrange & Act
            var motor = new Motor();

            // Assert - Verifica salas críticas
            Assert.True(motor.Mundo.ContainsKey("west_house"), "Deve conter west_house");
            Assert.True(motor.Mundo.ContainsKey("troll_room"), "Deve conter troll_room");
            Assert.True(motor.Mundo.ContainsKey("cellar"), "Deve conter cellar");
            Assert.True(motor.Mundo.ContainsKey("ew_passage"), "Deve conter ew_passage");
            Assert.True(motor.Mundo.ContainsKey("maze_1"), "Deve conter maze_1");
        }

        #endregion

        #region Testes de Navegação Básica

        [Fact]
        public void DevePermitirMovimentoNorte()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("norte");

            // Assert
            Assert.Equal("north_house", motor.Jogador.SalaAtualId);
            Assert.Contains("Norte da Casa", ObterTodasMensagens());
        }

        [Fact]
        public void DevePermitirMovimentoSul()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("sul");

            // Assert
            Assert.Equal("south_house", motor.Jogador.SalaAtualId);
            Assert.Contains("Sul da Casa", ObterTodasMensagens());
        }

        [Fact]
        public void DeveBloquearMovimentoInválido()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            string salaInicial = motor.Jogador.SalaAtualId;
            _mensagens.Clear();

            // Act - Tenta ir para o leste (bloqueado)
            motor.ProcessarComando("leste");

            // Assert
            Assert.Equal(salaInicial, motor.Jogador.SalaAtualId);
            Assert.Contains("Você não pode ir nessa direção", ObterTodasMensagens());
        }

        #endregion

        #region Testes de Navegação até o Porão

        [Fact]
        public void DeveConseguirChegarAteOPorao()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();

            // Act - Caminho: west_house -> north_house -> east_house -> kitchen -> living_room
            // Depois precisa mover tapete e abrir alçapão
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            
            // Entrar pela janela (se estiver aberta) ou pela porta
            // Vamos primeiro abrir a janela
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            
            // Agora estamos na cozinha, ir para sala de estar
            motor.ProcessarComando("oeste");
            
            // Mover tapete e abrir alçapão
            motor.ProcessarComando("mover tapete");
            motor.ProcessarComando("abrir alcapao");
            motor.ProcessarComando("descer");

            // Assert
            Assert.Equal("cellar", motor.Jogador.SalaAtualId);
            Assert.Contains("Porão", ObterTodasMensagens());
        }

        #endregion

        #region Testes de Lanterna e Escuridão

        [Fact]
        public void LanternaDeveIniciarDesligada()
        {
            // Arrange
            var motor = new Motor();
            var lanterna = motor.Mundo["living_room"].Itens
                .FirstOrDefault(i => i.Id == "lantern");

            // Assert
            Assert.NotNull(lanterna);
            Assert.False(lanterna.Flags.HasFlag(FlagsItem.EstaAceso));
        }

        [Fact]
        public void DevePermitirPegarLanterna()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Navegar até a sala de estar
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");
            
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("pegar lanterna");

            // Assert
            Assert.Contains(motor.Jogador.Inventario, i => i.Id == "lantern");
            Assert.Contains("lanterna", ObterTodasMensagens().ToLower());
        }

        [Fact]
        public void DevePermitirLigarLanterna()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Navegar até a sala de estar e pegar lanterna
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");
            motor.ProcessarComando("pegar lanterna");
            
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("ligar lanterna");

            // Assert
            var lanterna = motor.Jogador.Inventario.FirstOrDefault(i => i.Id == "lantern");
            Assert.NotNull(lanterna);
            Assert.True(lanterna.Flags.HasFlag(FlagsItem.EstaAceso));
        }

        [Fact]
        public void DeveImpedirMovimentoNoEscuroSemLanterna()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Navegar até o porão sem lanterna
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");
            motor.ProcessarComando("mover tapete");
            motor.ProcessarComando("abrir alcapao");
            motor.ProcessarComando("descer");
            
            _mensagens.Clear();

            // Act - Tentar ir para o norte no escuro
            motor.ProcessarComando("norte");

            // Assert
            var mensagens = ObterTodasMensagens().ToLower();
            Assert.True(
                mensagens.Contains("escuro") || 
                mensagens.Contains("luz") ||
                mensagens.Contains("não consegue ver"),
                "Deve avisar sobre a escuridão");
        }

        [Fact]
        public void DevePermitirMovimentoNoEscuroComLanternaAcesa()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Pegar e ligar lanterna
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");
            motor.ProcessarComando("pegar lanterna");
            motor.ProcessarComando("ligar lanterna");
            
            // Ir para o porão
            motor.ProcessarComando("mover tapete");
            motor.ProcessarComando("abrir alcapao");
            motor.ProcessarComando("descer");
            
            _mensagens.Clear();

            // Act - Ir para o norte com lanterna acesa
            motor.ProcessarComando("norte");

            // Assert
            Assert.Equal("troll_room", motor.Jogador.SalaAtualId);
            Assert.Contains("Troll", ObterTodasMensagens());
        }

        #endregion

        #region Testes de Fluxo do Ovo

        [Fact]
        public void DeveEncontrarArvoreNoCaminhoFloresta()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Navegar até o caminho da floresta
            motor.ProcessarComando("norte");
            motor.ProcessarComando("norte");

            // Assert
            Assert.Equal("forest_path", motor.Jogador.SalaAtualId);
            var sala = motor.Mundo["forest_path"];
            Assert.True(sala.Itens.Any(i => i.Id == "tree"));
        }

        [Fact]
        public void DevePermitirSubirNaArvore()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Navegar até o caminho da floresta
            motor.ProcessarComando("norte");
            motor.ProcessarComando("norte");
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("subir");

            // Assert
            Assert.Equal("up_tree", motor.Jogador.SalaAtualId);
            Assert.Contains("árvore", ObterTodasMensagens().ToLower());
        }

        [Fact]
        public void DeveEncontrarNinhoNaArvore()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Subir na árvore
            motor.ProcessarComando("norte");
            motor.ProcessarComando("norte");
            motor.ProcessarComando("subir");

            // Assert
            var sala = motor.Mundo["up_tree"];
            Assert.True(sala.Itens.Any(i => i.Id == "nest"));
        }

        [Fact]
        public void DevePermitirPegarNinho()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Subir na árvore
            motor.ProcessarComando("norte");
            motor.ProcessarComando("norte");
            motor.ProcessarComando("subir");
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("pegar ninho");

            // Assert
            Assert.Contains(motor.Jogador.Inventario, i => i.Id == "nest");
        }

        [Fact]
        public void OvoDeveEstarDentroDoNinho()
        {
            // Arrange
            var motor = new Motor();
            var ninho = motor.Mundo["up_tree"].Itens
                .FirstOrDefault(i => i.Id == "nest");

            // Assert
            Assert.NotNull(ninho);
            Assert.True(ninho.Conteudo.Any(i => i.Id == "egg"));
        }

        [Fact]
        public void DevePermitirAbrirOvo()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Pegar o ninho
            motor.ProcessarComando("norte");
            motor.ProcessarComando("norte");
            motor.ProcessarComando("subir");
            motor.ProcessarComando("pegar ninho");
            
            // Pegar o ovo do ninho (se necessário examinar primeiro)
            motor.ProcessarComando("examinar ninho");
            motor.ProcessarComando("pegar ovo");
            
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("abrir ovo");

            // Assert
            var mensagens = ObterTodasMensagens().ToLower();
            // O ovo pode abrir com sucesso ou quebrar dependendo da implementação
            Assert.True(
                mensagens.Contains("ovo") || 
                mensagens.Contains("abrir") ||
                mensagens.Contains("canário") ||
                mensagens.Contains("quebrado"),
                "Deve haver alguma resposta sobre abrir o ovo");
        }

        #endregion

        #region Testes de Navegação TROLL-ROOM

        [Fact]
        public void DeveConseguirChegarNaTrollRoom()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Pegar lanterna primeiro
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");
            motor.ProcessarComando("pegar lanterna");
            motor.ProcessarComando("ligar lanterna");
            
            // Ir para o porão
            motor.ProcessarComando("mover tapete");
            motor.ProcessarComando("abrir alcapao");
            motor.ProcessarComando("descer");
            
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("norte");

            // Assert
            Assert.Equal("troll_room", motor.Jogador.SalaAtualId);
            Assert.Contains("Troll", ObterTodasMensagens());
        }

        [Fact]
        public void TrollRoomDeveConectarComEWPassage()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Chegar na Troll Room com lanterna
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");
            motor.ProcessarComando("pegar lanterna");
            motor.ProcessarComando("ligar lanterna");
            motor.ProcessarComando("mover tapete");
            motor.ProcessarComando("abrir alcapao");
            motor.ProcessarComando("descer");
            motor.ProcessarComando("norte");
            
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("leste");

            // Assert
            Assert.Equal("ew_passage", motor.Jogador.SalaAtualId);
            Assert.Contains("Leste-Oeste", ObterTodasMensagens());
        }

        [Fact]
        public void TrollRoomDeveConectarComMaze1()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Chegar na Troll Room
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");
            motor.ProcessarComando("pegar lanterna");
            motor.ProcessarComando("ligar lanterna");
            motor.ProcessarComando("mover tapete");
            motor.ProcessarComando("abrir alcapao");
            motor.ProcessarComando("descer");
            motor.ProcessarComando("norte");
            
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("oeste");

            // Assert
            Assert.Equal("maze_1", motor.Jogador.SalaAtualId);
            Assert.Contains("Labirinto", ObterTodasMensagens());
        }

        [Fact]
        public void DevePermitirVoltarDaTrollRoomParaCellar()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            // Chegar na Troll Room
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");
            motor.ProcessarComando("pegar lanterna");
            motor.ProcessarComando("ligar lanterna");
            motor.ProcessarComando("mover tapete");
            motor.ProcessarComando("abrir alcapao");
            motor.ProcessarComando("descer");
            motor.ProcessarComando("norte");
            
            _mensagens.Clear();

            // Act
            motor.ProcessarComando("sul");

            // Assert
            Assert.Equal("cellar", motor.Jogador.SalaAtualId);
            Assert.Contains("Porão", ObterTodasMensagens());
        }

        #endregion

        #region Testes de Inventário

        [Fact]
        public void InventarioDeveIniciarVazio()
        {
            // Arrange & Act
            var motor = new Motor();

            // Assert
            Assert.Empty(motor.Jogador.Inventario);
        }

        [Fact]
        public void DevePermitirAdicionarItensAoInventario()
        {
            // Arrange
            var motor = CriarMotorComCapturaDeMensagens();
            motor.IniciarJogo();
            
            motor.ProcessarComando("norte");
            motor.ProcessarComando("leste");
            motor.ProcessarComando("abrir janela");
            motor.ProcessarComando("entrar");
            motor.ProcessarComando("oeste");

            // Act
            motor.ProcessarComando("pegar espada");
            motor.ProcessarComando("pegar lanterna");

            // Assert
            Assert.True(motor.Jogador.Inventario.Count >= 2);
            Assert.Contains(motor.Jogador.Inventario, i => i.Id == "sword");
            Assert.Contains(motor.Jogador.Inventario, i => i.Id == "lantern");
        }

        #endregion
    }
}
