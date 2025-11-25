# Correções de Bugs - ZorkBrasil

## Data: 2025-01-XX

### Resumo

Foram identificados e corrigidos 5 problemas relatados pelo usuário durante testes de gameplay. Todas as correções foram validadas contra o código original em ZIL (`EstruturaOriginal/mapa-salas.json`).

---

## 1. Porão Escuro com Lanterna Acesa

**Problema:** Ao entrar no porão com a lanterna acesa, o jogo exibia "Está breu. Você pode ser devorado por um Grue."

**Causa Raiz:** O método `Sala.ObterDescricao()` verificava se a sala tinha a flag `Iluminada` e retornava uma mensagem de escuridão, mesmo que o `Motor.DescreverSalaAtual()` já tivesse verificado que o jogador tinha uma fonte de luz.

**Solução:**
- **Arquivo:** `Game/Core/Modelos/Sala.cs`
- **Mudança:** Removida a verificação de escuridão de `ObterDescricao()`
- **Justificativa:** A lógica de iluminação agora é tratada exclusivamente por `Motor.DescreverSalaAtual()`, que verifica tanto a iluminação da sala quanto as fontes de luz do jogador antes de chamar `ObterDescricao()`.

```csharp
// ANTES (linhas 67-72)
if (!Flags.HasFlag(FlagsSala.Iluminada))
{
    return "Está breu. Você pode ser devorado por um Grue.";
}

// DEPOIS
// Removido - lógica de luz tratada pelo Motor
```

**Validação ZIL:** ✅ No código original, o CELLAR não tem LIGHTBIT e deve ser iluminado por LANTERN quando ONBIT está setado.

---

## 2. Interação com Estante de Troféus (Trophy Case)

**Problema:** Comandos como "abrir estante" e "examinar estante" retornavam "Não vejo isso aqui".

**Causa Raiz:**
1. Faltavam sinônimos em português
2. Faltavam flags `PodeAbrir` e `EstaAberto`

**Solução:**
- **Arquivo:** `Game/Core/Logica/Mapa.cs` (linhas ~119)
- **Mudanças:**
  - Adicionados sinônimos: "trophy", "case", "estante", "trofeu", "trofeus"
  - Adicionadas flags: `FlagsItem.PodeAbrir | FlagsItem.EstaAberto`

```csharp
// ANTES
var trofeuCase = new Item("trophy_case", "Estante de Troféus", 
    "Uma estante para guardar tesouros.", 
    FlagsItem.Container | FlagsItem.Cenario);

// DEPOIS
var trofeuCase = new Item("trophy_case", "Estante de Troféus", 
    "Uma estante para guardar tesouros.", 
    FlagsItem.Container | FlagsItem.Cenario | FlagsItem.PodeAbrir | FlagsItem.EstaAberto);
trofeuCase.AdicionarSinonimo("trophy", "case", "estante", "trofeu", "trofeus");
```

**Validação ZIL:** ✅ TROPHY-CASE tem flags: TRANSBIT, CONTBIT, NDESCBIT, TRYTAKEBIT, SEARCHBIT, OPENBIT (implícito em CONTBIT)

---

## 3. Chaminé na Cozinha

**Problema:** Comando "examinar chamine" retornava "Você não vê isso aqui".

**Causa Raiz:** A chaminé era descrita na sala mas não existia como objeto interativo.

**Solução:**
- **Arquivo:** `Game/Core/Logica/Mapa.cs` (linhas ~96-101)
- **Mudanças:**
  - Criado objeto chaminé com ID "chimney"
  - Adicionados sinônimos: "chimney", "chamine", "fireplace", "lareira"
  - Marcado como cenário: `FlagsItem.Cenario`
  - Adicionado à lista de itens da cozinha

```csharp
// NOVO
var chamine = new Item("chimney", "Chaminé", 
    "Uma ampla chaminé, escura e cheia de fuligem.", 
    FlagsItem.Cenario);
chamine.AdicionarSinonimo("chimney", "chamine", "fireplace", "lareira");
cozinha.Itens.Add(chamine);
```

**Validação ZIL:** ✅ CHIMNEY existe com flags: CLIMBBIT, NDESCBIT

---

## 4. Porta de Madeira com Inscrições Estranhas

**Problema:** A porta de madeira mencionada na descrição da sala de estar não era interativa.

**Causa Raiz:** A porta era descrita mas não implementada como objeto.

**Solução:**
- **Arquivo:** `Game/Core/Logica/Mapa.cs` (linhas ~121-122)
- **Mudanças:**
  - Criado objeto porta com ID "wooden_door"
  - Adicionados sinônimos: "wooden", "door", "porta", "madeira", "inscricoes", "runas"
  - Marcado como cenário: `FlagsItem.Cenario`
  - Adicionado à lista de itens da sala de estar

```csharp
// NOVO
var portaMadeira = new Item("wooden_door", "Porta de madeira", 
    "A porta de madeira está coberta com runas estranhas.", 
    FlagsItem.Cenario);
portaMadeira.AdicionarSinonimo("wooden", "door", "porta", "madeira", "inscricoes", "runas");
salaEstar.Itens.Add(portaMadeira);
```

**Validação ZIL:** ✅ WOODEN-DOOR existe com flags: READBIT, DOORBIT, NDESCBIT, TRANSBIT

---

## 5. Pegar Saco Marrom da Mesa

**Problema:** Comando "pegar saco" retornava "Não vejo isso aqui".

**Causas Raiz:**
1. Faltavam sinônimos em português ("saco", "marrom")
2. Bug no método `Motor.PegarItem()`: ao buscar itens em containers, não verificava o ID do item, apenas sinônimos

**Soluções:**

### A) Mapa.cs (linhas ~73-75)
```csharp
// ANTES
saco.AdicionarSinonimo("bag", "sack", "brown");

// DEPOIS
saco.AdicionarSinonimo("bag", "sack", "brown", "saco", "marrom");
saco.DescricaoNoChao = "Um saco marrom alongado cheirando a pimenta.";
```

### B) Motor.cs (linha ~422)
```csharp
// ANTES
var itemDentro = container.Conteudo.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem));

// DEPOIS
var itemDentro = container.Conteudo.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
```

**Validação ZIL:** ✅ SANDWICH-BAG (brown sack) tem flags: TAKEBIT, CONTBIT, BURNBIT

---

## Arquivos Modificados

1. `Game/Core/Modelos/Sala.cs`
   - Removida verificação de escuridão em `ObterDescricao()`

2. `Game/Core/Logica/Mapa.cs`
   - Adicionados sinônimos portugueses ao saco e estante de troféus
   - Criados objetos: chaminé e porta de madeira
   - Adicionadas propriedades `DescricaoNoChao` a saco e garrafa

3. `Game/Core/Logica/Motor.cs`
   - Corrigida busca de itens em containers na função `PegarItem()`

---

## Testes Recomendados

Execute os seguintes comandos para validar as correções:

```
> norte
> leste
> abrir janela
> entrar
> pegar saco          # Deve funcionar
> examinar chamine    # Deve funcionar
> oeste
> abrir estante       # Deve funcionar
> examinar porta      # Deve funcionar (porta de madeira)
> mover tapete
> abrir alcapao
> pegar lanterna
> ligar lanterna
> descer
> olhar               # Deve mostrar descrição do porão, NÃO "Está breu"
```

---

## Próximos Passos

1. Executar testes unitários existentes para garantir que as mudanças não quebraram funcionalidades
2. Considerar adicionar testes automatizados para estes cenários específicos
3. Continuar implementação de salas e conexões faltantes do mapa original

