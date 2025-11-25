# Estrutura Original do Zork I

Esta pasta contém arquivos de referência extraídos do código-fonte original do Zork I em ZIL (Zork Implementation Language).

## Arquivos

### mapa-salas.json
Definição completa do mundo do Zork I contendo:
- **110 salas** com descrições, saídas e propriedades
- **122 objetos** com atributos, sinônimos e interações
- Estrutura fiel ao código ZIL original do repositório [historicalsource/zork1](https://github.com/historicalsource/zork1)

### inimigos.json
Dados dos inimigos e NPCs do jogo, incluindo:
- Atributos de combate
- Comportamentos e padrões de ataque
- Itens que podem ser obtidos

### viewer.html
Visualizador interativo dos arquivos JSON para facilitar navegação e exploração da estrutura do jogo.

**Como usar:**
1. Inicie um servidor HTTP local na pasta do projeto:
   ```bash
   py -m http.server 8000
   ```
2. Acesse no navegador: http://localhost:8000/EstruturaOriginal/viewer.html

## Referências

- **Código Fonte Original (ZIL)**: https://github.com/historicalsource/zork1
- **Gameplay Completa**: https://www.youtube.com/watch?v=mWNgQdybISA
- **Documentação ZIL**: Os arquivos JSON foram extraídos e estruturados a partir dos arquivos `.zil` originais

## Propósito

Estes arquivos servem como referência autoritativa para garantir que a implementação em C# do ZorkBrasil seja fiel ao jogo original em:
- Descrições de salas e objetos
- Mecânicas de gameplay
- Puzzles e interações
- Estrutura do mundo