# ZorkBrasil - Testes Automatizados

## 📋 Resumo dos Testes

O projeto agora inclui **23 testes automatizados** usando xUnit para garantir que funcionalidades críticas não quebrem durante o desenvolvimento.

## 🏗️ Estrutura do Projeto

```
ZorkBrasil/
├── Game/                          # Código-fonte do jogo
│   ├── Core/                      # Lógica principal
│   ├── Interface/                 # Interface (futuro)
│   ├── Program.cs                 # Ponto de entrada
│   └── ZorkBrasil.Game.csproj     # Projeto do jogo
├── Tests/                         # Testes automatizados
│   ├── GameFlowTests.cs           # Testes de fluxo
│   └── ZorkBrasil.Tests.csproj    # Projeto de testes
├── EstruturaOriginal/             # Arquivos de referência ZIL
└── ZorkBrasil.sln                 # Solution principal
```

## 🧪 Executando os Testes

### Executar todos os testes
```bash
dotnet test
```

### Executar testes com saída detalhada
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Executar apenas o projeto de testes
```bash
dotnet test Tests/ZorkBrasil.Tests.csproj
```

## ✅ Testes Implementados

### 1. **Testes de Inicialização** (2 testes)
- ✅ Jogo deve iniciar na posição correta (west_house)
- ✅ Mundo deve carregar todas as salas críticas (TROLL-ROOM, CELLAR, etc.)

### 2. **Testes de Navegação Básica** (3 testes)
- ✅ Permitir movimento norte
- ✅ Permitir movimento sul
- ✅ Bloquear movimento inválido (com mensagem apropriada)

### 3. **Testes de Navegação até o Porão** (1 teste)
- ⚠️ Chegar até o porão (precisa ajustes na implementação)

### 4. **Testes de Lanterna e Escuridão** (5 testes)
- ✅ Lanterna deve iniciar desligada
- ✅ Permitir pegar lanterna
- ✅ Permitir ligar lanterna
- ⚠️ Impedir movimento no escuro sem lanterna (implementação em andamento)
- ✅ Permitir movimento no escuro com lanterna acesa

### 5. **Testes do Fluxo do Ovo** (7 testes)
- ✅ Encontrar árvore no caminho da floresta
- ✅ Permitir subir na árvore
- ✅ Encontrar ninho na árvore
- ✅ Permitir pegar ninho
- ✅ Ovo deve estar dentro do ninho
- ⚠️ Permitir abrir ovo (implementação em andamento)

### 6. **Testes de Navegação TROLL-ROOM** (4 testes)
- ✅ Conseguir chegar na TROLL-ROOM
- ✅ TROLL-ROOM deve conectar com EW-PASSAGE
- ✅ TROLL-ROOM deve conectar com MAZE-1
- ✅ Permitir voltar da TROLL-ROOM para CELLAR

### 7. **Testes de Inventário** (2 testes)
- ✅ Inventário deve iniciar vazio
- ✅ Permitir adicionar itens ao inventário

## 📊 Status Atual

**Total:** 23 testes  
**✅ Passando:** 19 testes (82.6%)  
**⚠️ Falhando:** 4 testes (17.4%)

### Testes que Precisam de Correções

1. **DeveConseguirChegarAteOPorao** - O caminho para o porão precisa de ajustes
2. **DeveImpedirMovimentoNoEscuroSemLanterna** - Lógica de escuridão precisa ser implementada
3. **DevePermitirAbrirOvo** - Comando "abrir" para o ovo precisa ser implementado
4. **DeveBloquearMovimentoInválido** - Mensagem de erro precisa ser ajustada

## 🎮 Executando o Jogo

```bash
cd Game
dotnet run
```

## 🔧 Compilando o Projeto

```bash
# Compilar toda a solution
dotnet build

# Compilar apenas o jogo
dotnet build Game/ZorkBrasil.Game.csproj

# Compilar apenas os testes
dotnet build Tests/ZorkBrasil.Tests.csproj
```

## 📝 Adicionando Novos Testes

Para adicionar novos testes, edite o arquivo `Tests/GameFlowTests.cs`:

```csharp
[Fact]
public void NomeDoSeuTeste()
{
    // Arrange - Preparação
    var motor = CriarMotorComCapturaDeMensagens();
    motor.IniciarJogo();

    // Act - Ação
    motor.ProcessarComando("seu comando aqui");

    // Assert - Verificação
    Assert.Equal("valor_esperado", motor.Jogador.SalaAtualId);
}
```

## 🚀 Integração Contínua

Os testes podem ser executados automaticamente em pipelines CI/CD:

```yaml
# Exemplo para GitHub Actions
- name: Run tests
  run: dotnet test --no-build --verbosity normal
```

## 📚 Referências

- **xUnit**: Framework de testes utilizado
- **Motor do Jogo**: Classe principal testável via comandos
- **Mapa**: Todas as salas e conexões baseadas no Zork I original

---

**Nota**: Os testes garantem que mudanças no código não quebrem funcionalidades existentes. Execute `dotnet test` regularmente durante o desenvolvimento!
