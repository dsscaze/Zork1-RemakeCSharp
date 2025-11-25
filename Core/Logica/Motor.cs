using System;
using System.Collections.Generic;
using System.Linq;
using ZorkBrasil.Core.Enums;
using ZorkBrasil.Core.Modelos;

namespace ZorkBrasil.Core.Logica
{
    public class Motor
    {
        public Dictionary<string, Sala> Mundo { get; private set; }
        public Jogador Jogador { get; private set; }
        
        // Evento para enviar texto para a Interface (Console ou GUI)
        public Action<string>? OnMensagem;

        public Motor()
        {
            Mundo = Mapa.CarregarMundo();
            // Inicia no "West of House"
            Jogador = new Jogador("west_house");
        }

        public void IniciarJogo()
        {
            OnMensagem?.Invoke("ZORK I: O Grande Império Subterrâneo (Versão C# PT-BR)");
            OnMensagem?.Invoke("Portado por Daniel Cazé (Usando Gemini) - 2025\n");
            DescreverSalaAtual();
        }

        public void ProcessarComando(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada)) return;

            var partes = LimparEntrada(entrada);
            if (partes.Count == 0) return;

            string verbo = partes[0];
            string substantivo = partes.Count > 1 ? partes[1] : "";

            bool acaoRealizada = false;

            switch (verbo)
            {
                // Movimento Direto (ex: "norte", "n")
                case "norte": case "n": Mover(Direcao.Norte); acaoRealizada = true; break;
                case "sul": case "s": Mover(Direcao.Sul); acaoRealizada = true; break;
                case "leste": case "l": Mover(Direcao.Leste); acaoRealizada = true; break;
                case "oeste": case "o": case "w": Mover(Direcao.Oeste); acaoRealizada = true; break;
                
                case "subir":
                    // Se o jogador digitar "subir arvore", tratamos como ir para Cima
                    if (substantivo == "arvore" || substantivo == "arv")
                        Mover(Direcao.Cima); 
                    else
                        Mover(Direcao.Cima); // "subir" sozinho também tenta ir para cima
                    break;

                case "descer": case "d": case "baixo": Mover(Direcao.Baixo); acaoRealizada = true; break;
                
                // Movimento Composto (ex: "ir norte", "entrar")
                case "ir": 
                case "andar":
                    InterpretarMovimento(substantivo); 
                    acaoRealizada = true; 
                    break;

                

                case "entrar":
                    // Tenta entrar (útil para janelas ou 'entrar casa')
                    Mover(Direcao.Entrar);
                    acaoRealizada = true;
                    break;

                case "sair":
                    Mover(Direcao.Sair);
                    acaoRealizada = true;
                    break;

                case "ligar":
                case "acender":
                    AlterarEstadoLuz(substantivo, ligar: true);
                    acaoRealizada = true;
                    break;

                case "desligar":
                case "apagar":
                    AlterarEstadoLuz(substantivo, ligar: false);
                    acaoRealizada = true;
                    break;
                
                // Ações
                case "olhar": DescreverSalaAtual(forcarCompleta: true); break;
                case "i":
                case "inventario": MostrarInventario(); break;
                
                case "pegar": 
                    PegarItem(substantivo); 
                    acaoRealizada = true; 
                    break;
                case "largar":
                case "drop": // manter compatibilidade com inglês é bom
                    LargarItem(substantivo);
                    acaoRealizada = true;
                    break;
                case "abrir": 
                    AbrirItem(substantivo); 
                    acaoRealizada = true; 
                    break;
                
                case "ler":
                case "read":
                    LerItem(substantivo);
                    acaoRealizada = true;
                    break;
                
                case "examinar":
                case "examinr":
                case "x":
                case "examine":
                    ExaminarItem(substantivo);
                    acaoRealizada = true;
                    break;
                
                case "jogar":
                case "arremessar":
                case "throw":
                    JogarItem(substantivo);
                    acaoRealizada = true;
                    break;
                
                case "pisar":
                case "escalar":
                case "climb":
                    PisarObjeto(substantivo);
                    acaoRealizada = true;
                    break;
                
                case "mover":
                case "empurrar":
                    MoverObjeto(substantivo);
                    acaoRealizada = true;
                    break;

                default:
                    OnMensagem?.Invoke("Eu não sei como fazer isso.");
                    break;
            }

            if (acaoRealizada)
            {
                Jogador.Turnos++;
            }
        }

        // Método auxiliar para traduzir "norte" string -> Direcao enum
        private void InterpretarMovimento(string direcaoTexto)
        {
            switch (direcaoTexto)
            {
                case "norte": case "n": Mover(Direcao.Norte); break;
                case "sul": case "s": Mover(Direcao.Sul); break;
                case "leste": case "l": Mover(Direcao.Leste); break;
                case "oeste": case "o": case "w": Mover(Direcao.Oeste); break;
                case "cima": case "u": Mover(Direcao.Cima); break;
                case "baixo": case "d": Mover(Direcao.Baixo); break;
                case "dentro": case "casa": Mover(Direcao.Entrar); break; // "ir dentro"
                case "fora": Mover(Direcao.Sair); break;
                default: OnMensagem?.Invoke("Ir para onde?"); break;
            }
        }

        private void AlterarEstadoLuz(string nomeItem, bool ligar)
        {
            // Procura no inventário ou na sala
            var sala = Mundo[Jogador.SalaAtualId];
            var item = Jogador.Inventario.Concat(sala.Itens)
                .FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);

            if (item == null)
            {
                OnMensagem?.Invoke("Não vejo isso aqui.");
                return;
            }

            // Verifica se é uma fonte de luz (Lanterna)
            if (!item.Flags.HasFlag(FlagsItem.FonteDeLuz))
            {
                OnMensagem?.Invoke("Isso não é algo que se possa ligar ou desligar.");
                return;
            }

            if (ligar)
            {
                if (item.Flags.HasFlag(FlagsItem.EstaAceso))
                {
                    OnMensagem?.Invoke("Já está aceso.");
                }
                else
                {
                    item.Flags |= FlagsItem.EstaAceso;
                    OnMensagem?.Invoke($"A {item.Nome} agora está brilhando.");
                    // Se estivermos numa sala escura, agora ela será descrita automaticamente no próximo loop
                    DescreverSalaAtual(); 
                }
            }
            else
            {
                if (!item.Flags.HasFlag(FlagsItem.EstaAceso))
                {
                    OnMensagem?.Invoke("Já está apagado.");
                }
                else
                {
                    item.Flags &= ~FlagsItem.EstaAceso; // Remove a flag
                    OnMensagem?.Invoke($"A {item.Nome} apagou.");
                    
                    // Se a sala for escura, o jogador ficará no breu agora
                    DescreverSalaAtual();
                }
            }
        }

        private List<string> LimparEntrada(string entrada)
        {
            // Remove palavras de ligação (Stop Words)
            var ignorar = new[] { "o", "a", "os", "as", "um", "uma", "do", "da", "para", "em", "no", "na" };
            
            return entrada.ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(p => !ignorar.Contains(p))
                .ToList();
        }

private void MoverObjeto(string nomeItem)
        {
            var sala = Mundo[Jogador.SalaAtualId];
            
            // Lógica específica: Mover Tapete na Sala de Estar
            if (sala.Id == "living_room" && (nomeItem == "tapete" || nomeItem == "rug"))
            {
                var tapete = sala.Itens.FirstOrDefault(i => i.Id == "rug");
                
                // Se o tapete já foi movido (verificamos se o alçapão já existe na sala)
                if (sala.Itens.Any(i => i.Id == "trap_door"))
                {
                    OnMensagem?.Invoke("Você já moveu o tapete.");
                    return;
                }

                if (tapete != null)
                {
                    OnMensagem?.Invoke("Com um grande esforço, você move o tapete para o lado da sala, revelando um alçapão empoeirado fechado.");
                    
                    // Atualiza a descrição do tapete
                    tapete.DescricaoNoChao = "O tapete está enrolado num canto da sala.";
                    
                    // CRIA E ADICIONA O ALÇAPÃO DINAMICAMENTE
                    var alcapao = new Item("trap_door", "Alçapão", "Um alçapão de madeira fechado.", FlagsItem.Cenario | FlagsItem.PodeAbrir);
                    alcapao.AdicionarSinonimo("alcapao", "porta");
                    sala.Itens.Add(alcapao);

                    // ATUALIZA A SAÍDA ou CRIA se não existir
                    if (sala.Saidas.ContainsKey(Direcao.Baixo))
                    {
                        sala.Saidas[Direcao.Baixo].MensagemBloqueio = "O alçapão está fechado.";
                    }
                    else
                    {
                        // Cria a saída para o porão, mas bloqueada
                        sala.DefinirSaida(Direcao.Baixo, "cellar", bloqueada: true, msgBloqueio: "O alçapão está fechado.");
                    }
                }
                return;
            }

            OnMensagem?.Invoke("Mover isso não adianta nada.");
        }

        private void Mover(Direcao direcao)
        {
            var salaAtual = Mundo[Jogador.SalaAtualId];

            if (salaAtual.Saidas.ContainsKey(direcao))
            {
                var saida = salaAtual.Saidas[direcao];
                
                if (saida.EstaBloqueada)
                {
                    bool desbloqueado = false;

                    // Regra: Janela da Cozinha/Fundos
                    if (salaAtual.Id == "east_house" && (direcao == Direcao.Oeste || direcao == Direcao.Entrar))
                    {
                        var janela = salaAtual.Itens.FirstOrDefault(i => i.Id == "window");
                        if (janela != null && janela.Flags.HasFlag(FlagsItem.EstaAberto)) desbloqueado = true;
                    }

                    // NOVO: Regra do Alçapão (Living Room -> Baixo)
                    if (salaAtual.Id == "living_room" && direcao == Direcao.Baixo)
                    {
                        var alcapao = salaAtual.Itens.FirstOrDefault(i => i.Id == "trap_door");
                        if (alcapao != null && alcapao.Flags.HasFlag(FlagsItem.EstaAberto)) desbloqueado = true;
                    }

                    // NOVO: Regra do Alçapão (Cellar -> Cima) - Assumindo que se abriu em cima, abre embaixo
                    // Para simplificar, vamos assumir que o alçapão é mágico e sincroniza, ou destrancamos na hora.
                    // Mas como o item "alcapao" está na sala de cima, se estivermos embaixo, precisamos checar o estado "global" ou simplificar.
                    // Simplificação: Se conseguiu descer, pode subir.
                    if (salaAtual.Id == "cellar" && direcao == Direcao.Cima)
                    {
                        // Se você está no porão, o alçapão deve estar aberto lá em cima. 
                        // Num sistema real, o objeto alçapão deveria ser compartilhado entre as salas.
                        desbloqueado = true; 
                    }

                    if (desbloqueado)
                    {
                        Jogador.SalaAtualId = saida.SalaDestinoId;
                        DescreverSalaAtual();
                    }
                    else
                    {
                        OnMensagem?.Invoke(saida.MensagemBloqueio ?? "O caminho está bloqueado.");
                    }
                }
                else
                {
                    Jogador.SalaAtualId = saida.SalaDestinoId;
                    DescreverSalaAtual();
                }
            }
            else
            {
                OnMensagem?.Invoke("Você não pode ir para lá.");
            }
        }
        private void DescreverSalaAtual(bool forcarCompleta = false)
        {
            var sala = Mundo[Jogador.SalaAtualId];
            
            // --- LÓGICA DE ESCURIDÃO ---
            bool salaIluminada = sala.Flags.HasFlag(FlagsSala.Iluminada);
            bool jogadorTemLuz = Jogador.Inventario.Any(i => i.EmiteLuz()); // Precisamos garantir que a lanterna esteja ACESA
            // Nota: No Item.cs, EmiteLuz() deve retornar (FonteDeLuz && EstaAceso)

            // Simplificação para este passo: Vamos assumir que se pegar a lanterna ela ilumina
            // Se quiser ser rigoroso: jogadorTemLuz = Jogador.Inventario.Any(i => i.Id == "lantern" && i.Flags.HasFlag(FlagsItem.EstaAceso));

            if (!salaIluminada && !jogadorTemLuz)
            {
                OnMensagem?.Invoke($"\n=== Escuridão ===");
                OnMensagem?.Invoke("Está um breu total. É provável que você seja devorado por um Grue.");
                return; // Não mostra descrição nem itens!
            }
            // ---------------------------

            OnMensagem?.Invoke($"\n{sala.Nome}");
            if (forcarCompleta) sala.Flags &= ~FlagsSala.Visitada;
            OnMensagem?.Invoke(sala.ObterDescricao());

            // Lógica especial para UP-A-TREE: mostrar itens no chão do PATH
            if (sala.Id == "up_tree" && Mundo.ContainsKey("forest_path"))
            {
                var pathRoom = Mundo["forest_path"];
                if (pathRoom.Itens.Count > 0)
                {
                    OnMensagem?.Invoke("No chão embaixo de você, você pode ver:");
                    foreach (var item in pathRoom.Itens)
                    {
                        OnMensagem?.Invoke($"  - {item.Nome}");
                    }
                }
            }

            if (sala.Itens.Count > 0)
            {
                OnMensagem?.Invoke("Você vê aqui:");
                foreach (var item in sala.Itens)
                {
                    // Não mostra o item se for cenário fixo e não tiver descrição de chão (opcional)
                    OnMensagem?.Invoke($"  - {item.DescricaoNoChao ?? item.Nome}");
                    
                    // Se é um container aberto, mostra o conteúdo
                    if (item.EhContainer() && item.EstaAberto() && item.Conteudo.Count > 0)
                    {
                        foreach (var itemDentro in item.Conteudo)
                        {
                            OnMensagem?.Invoke($"    {itemDentro.DescricaoNoChao ?? itemDentro.Nome}");
                        }
                    }
                }
            }
        }

        private void PegarItem(string nomeItem)
        {
            var sala = Mundo[Jogador.SalaAtualId];
            
            // Procura na sala
            var itemNaSala = sala.Itens.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);

            if (itemNaSala != null)
            {
                if (itemNaSala.Flags.HasFlag(FlagsItem.Pegavel))
                {
                    sala.Itens.Remove(itemNaSala);
                    Jogador.AdicionarItem(itemNaSala);
                    OnMensagem?.Invoke($"Você pegou: {itemNaSala.Nome}");
                }
                else
                {
                    OnMensagem?.Invoke("Você não pode pegar isso.");
                }
                return;
            }

            // Procura dentro de containers ABERTOS na sala
            foreach(var container in sala.Itens.Where(i => i.EhContainer() && i.EstaAberto()))
            {
                var itemDentro = container.Conteudo.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem));
                if (itemDentro != null)
                {
                    container.Conteudo.Remove(itemDentro);
                    Jogador.AdicionarItem(itemDentro);
                    OnMensagem?.Invoke($"Você pegou {itemDentro.Nome} de {container.Nome}.");
                    return;
                }
            }

            OnMensagem?.Invoke("Não vejo isso aqui.");
        }

        private void AbrirItem(string nomeItem)
        {
            var sala = Mundo[Jogador.SalaAtualId];
            
            // Procura item na sala (chão/cenário) OU no inventário
            var item = sala.Itens.Concat(Jogador.Inventario)
                .FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);

            if (item == null)
            {
                OnMensagem?.Invoke("Não vejo isso aqui.");
                return;
            }

            // Lógica especial para o OVO
            if (item.Id == "egg")
            {
                AbrirOvo(item);
                return;
            }

            if (!item.Flags.HasFlag(FlagsItem.PodeAbrir))
            {
                OnMensagem?.Invoke("Isso não pode ser aberto.");
                return;
            }

            if (item.Flags.HasFlag(FlagsItem.EstaAberto))
            {
                OnMensagem?.Invoke("Já está aberto.");
                return;
            }

            // Abre o item
            item.Flags |= FlagsItem.EstaAberto;
            
            // Mensagem customizada para cenários
            if (item.Id == "window")
                OnMensagem?.Invoke("Com grande esforço, você abre a janela.");
            else
                OnMensagem?.Invoke($"Você abriu: {item.Nome}.");

            if(item.Conteudo.Any()) 
                OnMensagem?.Invoke("Dentro há: " + string.Join(", ", item.Conteudo.Select(x=>x.Nome)));
        }

        private void AbrirOvo(Item ovo)
        {
            // ZIL: (<FSET? ,PRSO ,OPENBIT> ...)
            if (ovo.Flags.HasFlag(FlagsItem.EstaAberto))
            {
                OnMensagem?.Invoke("O ovo já está aberto.");
                return;
            }

            // ZIL: (<NOT ,PRSI> ...)
            // No original, você NÃO pode abrir o ovo apenas com as mãos
            // Precisa usar uma ferramenta ou arma, mas isso danifica o ovo
            // Por enquanto, apenas bloqueamos a abertura
            OnMensagem?.Invoke("Você não tem nem as ferramentas nem a experiência.");
            
            // TODO: Implementar "abrir X com Y" syntax
            // Se usar ferramenta/arma: AbrirOvoDanificado()
        }

        private void AbrirOvoDanificado()
        {
            // ZIL: BAD-EGG routine
            var sala = Mundo[Jogador.SalaAtualId];
            
            // Procura ovo na sala ou inventário
            var ovo = sala.Itens.Concat(Jogador.Inventario)
                .FirstOrDefault(i => i.Id == "egg");
            
            if (ovo == null) return;
            
            var localizacao = ovo.Localizacao(Jogador, sala);
            
            OnMensagem?.Invoke("O ovo agora está aberto, mas a falta de jeito de sua tentativa comprometeu seriamente seu apelo estético.");
            
            // Substitui egg por broken_egg
            var ovoQuebrado = CriarOvoQuebrado();
            
            if (localizacao == "inventario")
            {
                Jogador.Inventario.Remove(ovo);
                Jogador.AdicionarItem(ovoQuebrado);
            }
            else
            {
                sala.Itens.Remove(ovo);
                sala.Itens.Add(ovoQuebrado);
            }
        }

        private Item CriarOvoQuebrado()
        {
            var ovoQuebrado = new Item("broken_egg", "Ovo quebrado com joias", 
                "O ovo agora está aberto, mas a falta de jeito de sua tentativa comprometeu seriamente seu apelo estético.",
                FlagsItem.Pegavel | FlagsItem.Container | FlagsItem.EstaAberto);
            ovoQuebrado.AdicionarSinonimo("ovo", "egg", "broken");
            
            var canarioQuebrado = new Item("broken_canary", "Canário de corda quebrado",
                "Há um canário de corda dourado aninhado no ovo. Ele parece ter tido recentemente uma experiência ruim. " +
                "As montagens para seus olhos como joias estão vazias, e seu bico de prata está amassado. " +
                "Através de uma janela de cristal rachada abaixo de sua asa esquerda, você pode ver os restos " +
                "de uma maquinaria intrincada. Pode ter algum valor como curiosidade.",
                FlagsItem.Pegavel);
            canarioQuebrado.AdicionarSinonimo("canario", "canário", "passaro", "pássaro", "bird", "broken");
            
            ovoQuebrado.Conteudo.Add(canarioQuebrado);
            return ovoQuebrado;
        }

        private void JogarItem(string nomeItem)
        {
            if (string.IsNullOrWhiteSpace(nomeItem))
            {
                OnMensagem?.Invoke("Jogar o quê?");
                return;
            }

            var item = Jogador.Inventario.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
            
            if (item == null)
            {
                OnMensagem?.Invoke("Você não está carregando isso.");
                return;
            }

            // ZIL: Special handling for EGG
            if (item.Id == "egg")
            {
                var sala = Mundo[Jogador.SalaAtualId];
                Jogador.Inventario.Remove(item);
                
                OnMensagem?.Invoke("Seu manuseio bastante indelicado do ovo causou algum dano a ele, embora você tenha conseguido abri-lo.");
                
                // Cria ovo quebrado na sala
                var ovoQuebrado = CriarOvoQuebrado();
                sala.Itens.Add(ovoQuebrado);
                return;
            }

            // Comportamento padrão
            Jogador.Inventario.Remove(item);
            var salaAtual = Mundo[Jogador.SalaAtualId];
            salaAtual.Itens.Add(item);
            OnMensagem?.Invoke("Jogado.");
        }

        private void PisarObjeto(string nomeItem)
        {
            if (string.IsNullOrWhiteSpace(nomeItem))
            {
                OnMensagem?.Invoke("Pisar em quê?");
                return;
            }

            var sala = Mundo[Jogador.SalaAtualId];
            var item = sala.Itens.Concat(Jogador.Inventario)
                .FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);

            if (item == null)
            {
                OnMensagem?.Invoke("Não vejo isso aqui.");
                return;
            }

            // ZIL: EGG special handling for CLIMB-ON/HATCH
            if (item.Id == "egg")
            {
                OnMensagem?.Invoke("Há um rangido perceptível vindo de baixo de você, e a inspeção revela que o ovo está deitado aberto, seriamente danificado.");
                
                // Remove ovo e coloca quebrado
                if (Jogador.Inventario.Contains(item))
                {
                    Jogador.Inventario.Remove(item);
                    Jogador.AdicionarItem(CriarOvoQuebrado());
                }
                else
                {
                    sala.Itens.Remove(item);
                    sala.Itens.Add(CriarOvoQuebrado());
                }
                return;
            }

            OnMensagem?.Invoke("Isso seria inútil.");
        }

        private void ExaminarItem(string nomeItem)
        {
            if (string.IsNullOrWhiteSpace(nomeItem))
            {
                OnMensagem?.Invoke("Examinar o quê?");
                return;
            }

            var sala = Mundo[Jogador.SalaAtualId];
            Item? item = null;

            // 1. Procura no inventário
            item = Jogador.Inventario.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);

            // 2. Se não achou, procura na sala
            if (item == null)
            {
                item = sala.Itens.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
            }

            // 3. Se não achou, busca dentro de containers abertos na sala
            if (item == null)
            {
                foreach(var container in sala.Itens.Where(i => i.EhContainer() && i.EstaAberto()))
                {
                    item = container.Conteudo.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
                    if (item != null) break;
                }
            }

            // 4. Se não achou, busca dentro de containers abertos no inventário
            if (item == null)
            {
                foreach(var container in Jogador.Inventario.Where(i => i.EhContainer() && i.EstaAberto()))
                {
                    item = container.Conteudo.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
                    if (item != null) break;
                }
            }

            if (item == null)
            {
                OnMensagem?.Invoke("Você não vê isso aqui.");
                return;
            }

            // Exibe a descrição do item
            OnMensagem?.Invoke(item.Descricao);

            // Se for um container aberto com conteúdo, mostra o que tem dentro
            if (item.EhContainer() && item.EstaAberto() && item.Conteudo.Any())
            {
                OnMensagem?.Invoke("Dentro há: " + string.Join(", ", item.Conteudo.Select(x => x.Nome)));
            }
        }

        private void LargarItem(string nomeItem)
        {
            var item = Jogador.RemoverItem(nomeItem);
            if (item != null)
            {
                var sala = Mundo[Jogador.SalaAtualId];
                
                // ZIL: Special handling when dropping NEST with EGG from UP-A-TREE
                if (item.Id == "nest" && sala.Id == "up_tree")
                {
                    var ovo = item.Conteudo.FirstOrDefault(i => i.Id == "egg");
                    if (ovo != null)
                    {
                        OnMensagem?.Invoke("O ninho cai no chão, e o ovo escorrega dele, seriamente danificado.");
                        
                        // Remove ovo do ninho
                        item.Conteudo.Remove(ovo);
                        
                        // Cria ovo quebrado no PATH (chão da floresta)
                        var caminhoFloresta = Mundo["forest_path"];
                        caminhoFloresta.Itens.Add(CriarOvoQuebrado());
                        
                        // Adiciona ninho vazio na sala
                        sala.Itens.Add(item);
                        return;
                    }
                }
                
                // ZIL: Special handling when dropping EGG from UP-A-TREE
                if (item.Id == "egg" && sala.Id == "up_tree")
                {
                    OnMensagem?.Invoke("O ovo cai no chão e se abre, seriamente danificado.");
                    
                    // Cria ovo quebrado no PATH
                    var caminhoFloresta = Mundo["forest_path"];
                    caminhoFloresta.Itens.Add(CriarOvoQuebrado());
                    return;
                }
                
                sala.Itens.Add(item);
                OnMensagem?.Invoke($"Largado: {item.Nome}");
            }
            else
            {
                OnMensagem?.Invoke("Você não está carregando isso.");
            }
        }

        private void MostrarInventario()
        {
            if (Jogador.Inventario.Count == 0)
            {
                OnMensagem?.Invoke("Você não está carregando nada.");
            }
            else
            {
                OnMensagem?.Invoke("Você está carregando:");
                foreach(var item in Jogador.Inventario)
                {
                    OnMensagem?.Invoke($" - {item.Nome}");
                }
            }
        }

        private void LerItem(string nomeItem)
        {
            var sala = Mundo[Jogador.SalaAtualId];
            
            // Procura item no inventário, na sala (chão) ou dentro de containers ABERTOS
            Item? item = null;
            bool itemEstavaNoInventario = false;
            Item? containerOrigem = null;
            
            // 1. Busca no inventário
            item = Jogador.Inventario.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
            if (item != null)
                itemEstavaNoInventario = true;
            
            // 2. Se não achou, busca na sala (chão)
            if (item == null)
                item = sala.Itens.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
            
            // 3. Se não achou, busca dentro de containers abertos na sala
            if (item == null)
            {
                foreach(var container in sala.Itens.Where(i => i.EhContainer() && i.EstaAberto()))
                {
                    item = container.Conteudo.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
                    if (item != null) 
                    {
                        containerOrigem = container;
                        break;
                    }
                }
            }
            
            // 4. Se não achou, busca dentro de containers abertos no inventário
            if (item == null)
            {
                foreach(var container in Jogador.Inventario.Where(i => i.EhContainer() && i.EstaAberto()))
                {
                    item = container.Conteudo.FirstOrDefault(i => i.Sinonimos.Contains(nomeItem) || i.Id == nomeItem);
                    if (item != null) 
                    {
                        containerOrigem = container;
                        itemEstavaNoInventario = true;
                        break;
                    }
                }
            }

            if (item == null)
            {
                OnMensagem?.Invoke("Não vejo isso aqui.");
                return;
            }

            if (!item.Flags.HasFlag(FlagsItem.Legivel))
            {
                OnMensagem?.Invoke("Isso não tem nada escrito para ler.");
                return;
            }

            // Se o item não está no inventário E é pegável, pega automaticamente
            if (!itemEstavaNoInventario && item.EhPegavel())
            {
                // Remove do container ou da sala
                if (containerOrigem != null)
                    containerOrigem.Conteudo.Remove(item);
                else
                    sala.Itens.Remove(item);
                
                Jogador.AdicionarItem(item);
                OnMensagem?.Invoke($"(pegando o {item.Nome})");
            }

            // Exibe o conteúdo do item (no Zork original, cada item legível tem um texto específico)
            ExibirTextoLegivel(item);
        }

        private void ExibirTextoLegivel(Item item)
        {
            // Aqui você pode ter um switch case ou um dicionário para textos específicos
            switch(item.Id)
            {
                case "leaflet":
                    OnMensagem?.Invoke("\n\"BEM-VINDO AO ZORK!\"");
                    OnMensagem?.Invoke("\nZORK é um jogo de aventura e exploração.");
                    OnMensagem?.Invoke("No Zork, a pessoa jogadora é um aventureiro ousado em busca de tesouros e glória.");
                    OnMensagem?.Invoke("Você precisará se mover entre locais, coletar objetos, resolver enigmas e derrotar inimigos.");
                    OnMensagem?.Invoke("\nComandos úteis:");
                    OnMensagem?.Invoke("  INVENTARIO - mostra o que você está carregando");
                    OnMensagem?.Invoke("  OLHAR - descreve sua localização atual");
                    OnMensagem?.Invoke("  NORTE, SUL, LESTE, OESTE - move entre locais");
                    OnMensagem?.Invoke("  PEGAR, LARGAR, ABRIR, LER - interage com objetos");
                    OnMensagem?.Invoke("\nBoa sorte!\n");
                    break;
                
                default:
                    OnMensagem?.Invoke(item.Descricao);
                    break;
            }
        }
    }
}