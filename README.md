# ZorkBrasil

Uma adaptação em C# e Português Brasileiro do clássico jogo de aventura em texto **Zork I** (1980).

## Sobre o Projeto

ZorkBrasil é um port fiel do jogo original Zork I, traduzido completamente para o português brasileiro. O projeto utiliza o código-fonte histórico disponível no repositório [historicalsource/zork1](https://github.com/historicalsource/zork1) (MIT License) como referência para garantir autenticidade nas descrições, mecânicas e objetos.

## Como Jogar

### Requisitos
- .NET 8.0 SDK ou superior

### Executar o Jogo

```bash
cd ZorkBrasil
dotnet run
```

### Comandos Básicos

- **Movimentação**: `norte`, `sul`, `leste`, `oeste`, `cima`, `baixo`, `entrar`, `sair`
- **Interação com Objetos**: 
  - `pegar [objeto]` - Pega um item
  - `largar [objeto]` - Larga um item do inventário
  - `examinar [objeto]` - Examina um item ou sala
  - `abrir [objeto]` - Abre containers ou portas
  - `fechar [objeto]` - Fecha containers ou portas
  - `inventario` ou `i` - Mostra o inventário
- **Outros Comandos**:
  - `olhar` - Mostra a descrição completa da sala atual
  - `sair` - Sai do jogo

## Estrutura do Projeto

```
ZorkBrasil/
├── Core/                           # Núcleo do motor do jogo
│   ├── Enums/                      
│   │   ├── Direcao.cs             # Enum para direções de movimento (Norte, Sul, Leste, Oeste, Cima, Baixo)
│   │   ├── Flags.cs               # Flags de estado das salas (Iluminada, Interior, Visitada)
│   │   └── FlagsItem.cs           # Flags de propriedades dos itens (Pegavel, Container, PodeAbrir, FonteDeLuz, etc)
│   │
│   ├── Logica/                     
│   │   ├── Mapa.cs                # Definição completa do mundo: 18 salas, objetos, conexões e itens complexos
│   │   └── Motor.cs               # Engine principal: parser de comandos, loop do jogo, lógica de interação
│   │
│   └── Modelos/                    
│       ├── Item.cs                # Classe para objetos interativos (propriedades, sinônimos, conteúdo)
│       ├── Jogador.cs             # Estado do jogador (inventário, sala atual, capacidade)
│       ├── Saida.cs               # Conexões entre salas (direção, destino, bloqueio)
│       └── Sala.cs                # Salas do jogo (descrição, itens, saídas, flags de estado)
│
├── Interface/                      # (Reservado para futura interface gráfica)
├── bin/                            # Arquivos compilados (ignorado pelo Git)
├── obj/                            # Arquivos temporários de build (ignorado pelo Git)
│
├── Program.cs                      # Ponto de entrada da aplicação
├── ZorkBrasil.csproj              # Configuração do projeto .NET
├── ZorkBrasil.sln                 # Solução Visual Studio
├── .gitignore                     # Arquivos ignorados pelo controle de versão
└── README.md                      # Este arquivo
```

### Arquitetura

O projeto segue uma arquitetura em camadas simples:

- **Core/Modelos**: Classes de domínio que representam os elementos do jogo
- **Core/Enums**: Enumerações e flags para estados e propriedades
- **Core/Logica**: Lógica de negócio, motor do jogo e definição do mundo
- **Program.cs**: Inicialização e loop principal da aplicação

O motor do jogo (`Motor.cs`) processa comandos em português e inglês, gerencia estado do jogador e aplica regras do jogo original Zork I.

## Funcionalidades Implementadas

- Sistema de salas e navegação
- Inventário e manipulação de objetos
- Containers (abrir/fechar/adicionar conteúdo)
- Sistema de iluminação (lanterna necessária em salas escuras)
- Comando examinar com descrições detalhadas
- Mecânicas complexas (ovo com joias, ninho, canário)
- Traduções completas do texto original ZIL

## Locais Implementados

### Área da Casa Branca
- Oeste da Casa (ponto de partida)
- Norte da Casa
- Sul da Casa
- Atrás da Casa (Leste)
- Cozinha
- Sala de Estar
- Sótão
- Porão
- Estúdio
- Galeria

### Área da Floresta
- Floresta 1, 2 e 3
- Caminho da Floresta
- Em Cima da Árvore
- Clareira
- Clareira com Grade
- Monte de Pedras

## Licença

Este projeto é uma adaptação não-oficial do Zork I original. O código-fonte original do Zork está disponível sob licença MIT no repositório [historicalsource/zork1](https://github.com/historicalsource/zork1).

**Zork** é uma marca registrada da Activision Publishing, Inc.

## Créditos

- **Jogo Original**: Criado por Tim Anderson, Marc Blank, Bruce Daniels e Dave Lebling (Infocom, 1980)
- **Código Fonte ZIL Original**: [github.com/historicalsource/zork1](https://github.com/historicalsource/zork1)
- **Adaptação C#**: ZorkBrasil Project

## Status do Desenvolvimento

Este projeto está em desenvolvimento ativo. Mais salas, objetos e mecânicas serão adicionados conforme o progresso da adaptação.

### Próximas Implementações
- Mais salas do mundo subterrâneo
- Sistema de combate com o Troll
- Mais puzzles e tesouros
- Sistema de pontuação
- Save/Load de jogo

---

**Bem-vindo ao Grande Império Subterrâneo!**
