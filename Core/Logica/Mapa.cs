using System.Collections.Generic;
using ZorkBrasil.Core.Enums;
using ZorkBrasil.Core.Modelos;

namespace ZorkBrasil.Core.Logica
{
    public static class Mapa
    {
        public static Dictionary<string, Sala> CarregarMundo()
        {
            var mundo = new Dictionary<string, Sala>();

            // =========================================================================
            // 1. CRIAÇÃO DE ITENS COMPARTILHADOS E ITENS COMPLEXOS
            // =========================================================================

            // A Janela dos fundos (Item existe na sala 'east_house' e 'kitchen')
            var janelaFundos = new Item("window", "Janela", "Uma janela que parece abrir.", 
                FlagsItem.Cenario | FlagsItem.PodeAbrir); // Começa fechada
            janelaFundos.AdicionarSinonimo("janela");

            // =========================================================================
            // 2. CRIAÇÃO DAS SALAS E SEUS ITENS
            // =========================================================================

            // --- OESTE DA CASA (INÍCIO) ---
            var oesteCasa = new Sala("west_house", "Oeste da Casa",
                "Você está em um campo aberto a oeste de uma casa branca, com uma porta da frente tapada com tábuas.");
            oesteCasa.Flags = FlagsSala.Iluminada;

            var panfleto = new Item("leaflet", "Panfleto", "Um panfleto promocional.", FlagsItem.Pegavel | FlagsItem.Legivel);
            panfleto.AdicionarSinonimo("panfleto", "folheto", "leaflet");
            
            var caixaCorreio = new Item("mailbox", "Caixa de Correio", "Uma pequena caixa de correio.", FlagsItem.Container | FlagsItem.PodeAbrir);
            caixaCorreio.AdicionarSinonimo("caixa", "correio", "mailbox");
            caixaCorreio.Conteudo.Add(panfleto);
            oesteCasa.Itens.Add(caixaCorreio);


            // --- NORTE DA CASA ---
            var norteCasa = new Sala("north_house", "Norte da Casa", 
                "Você está de frente para o lado norte de uma casa branca. Não há portas aqui, e todas as janelas estão tapadas. " +
                "Ao norte, um caminho estreito serpenteia através das árvores.");
            norteCasa.Flags = FlagsSala.Iluminada;


            // --- SUL DA CASA ---
            var sulCasa = new Sala("south_house", "Sul da Casa", 
                "Você está de frente para o lado sul de uma casa branca. Não há portas aqui, e todas as janelas estão tapadas com tábuas.");
            sulCasa.Flags = FlagsSala.Iluminada;


            // --- ATRÁS DA CASA (LESTE) ---
            var lesteCasa = new Sala("east_house", "Atrás da Casa",
                "Você está atrás da casa branca. Um caminho leva à floresta para o leste.\n" +
                "Em um canto da casa, há uma pequena janela.");
            lesteCasa.Flags = FlagsSala.Iluminada;
            lesteCasa.Itens.Add(janelaFundos); // Adiciona a janela


            // --- COZINHA ---
            var cozinha = new Sala("kitchen", "Cozinha",
                "Você está na cozinha da casa branca. Uma mesa parece ter sido usada recentemente para preparar comida. " +
                "Uma passagem leva a oeste e uma escada escura pode ser vista subindo. Uma chaminé escura leva para baixo " +
                "e a leste há uma pequena janela que está aberta.");
            cozinha.Flags = FlagsSala.Iluminada | FlagsSala.Interior;
            
            // Mesa da cozinha (KITCHEN-TABLE) - container aberto, superfície
            var mesaCozinha = new Item("kitchen_table", "Mesa da cozinha", "Uma mesa de madeira usada para preparar comida.", 
                FlagsItem.Cenario | FlagsItem.Container | FlagsItem.EstaAberto);
            mesaCozinha.AdicionarSinonimo("mesa", "table");
            
            // Itens da Cozinha
            var saco = new Item("sack", "Saco marrom", "Um saco marrom alongado cheirando a pimenta.", 
                FlagsItem.Pegavel | FlagsItem.Container | FlagsItem.PodeAbrir | FlagsItem.EstaAberto);
            saco.AdicionarSinonimo("bag", "sack", "brown");
            
            var alho = new Item("garlic", "Dente de alho", "Um dente de alho.", FlagsItem.Pegavel | FlagsItem.Comestivel);
            alho.AdicionarSinonimo("alho", "garlic", "clove");
            saco.Conteudo.Add(alho);

            var garrafa = new Item("bottle", "Garrafa de vidro", "Uma garrafa de vidro transparente.", 
                FlagsItem.Pegavel | FlagsItem.Container | FlagsItem.PodeAbrir | FlagsItem.EstaAberto);
            garrafa.AdicionarSinonimo("bottle", "garrafa", "glass");
            
            // Água dentro da garrafa
            var agua = new Item("water", "Água", "Água clara e potável.", FlagsItem.Comestivel);
            agua.AdicionarSinonimo("water", "agua", "h2o");
            garrafa.Conteudo.Add(agua);
            
            mesaCozinha.Conteudo.Add(saco);
            mesaCozinha.Conteudo.Add(garrafa);
            
            cozinha.Itens.Add(mesaCozinha);
            cozinha.Itens.Add(janelaFundos); // Mesma janela do lado de fora


            // --- SALA DE ESTAR ---
            var salaEstar = new Sala("living_room", "Sala de Estar",
                "Você está na sala de estar. Há uma porta para o leste, uma porta de madeira com inscrições estranhas para o oeste " +
                "que está pregada, e um tapete grande no centro da sala.");
            salaEstar.Flags = FlagsSala.Iluminada | FlagsSala.Interior;

            var espada = new Item("sword", "Espada Élfica", "Uma espada de brilho azulado.", FlagsItem.Pegavel | FlagsItem.Arma);
            espada.AdicionarSinonimo("espada", "lamina");

            // A Lanterna começa APAGADA (Sem flag EstaAceso)
            var lanterna = new Item("lantern", "Lanterna de latão", "Uma lanterna alimentada por bateria.", FlagsItem.Pegavel | FlagsItem.FonteDeLuz);
            lanterna.AdicionarSinonimo("lanterna", "lampada", "luz");

            var trofeuCase = new Item("trophy_case", "Estante de Troféus", "Uma estante para guardar tesouros.", FlagsItem.Container | FlagsItem.Cenario);

            var tapete = new Item("rug", "Tapete persa", "Um tapete persa muito caro e pesado.", FlagsItem.Cenario);
            tapete.AdicionarSinonimo("tapete");
            // OBS: O alçapão ("trap_door") não é adicionado aqui. Ele é criado pelo Motor quando se move o tapete.

            salaEstar.Itens.Add(espada);
            salaEstar.Itens.Add(lanterna);
            salaEstar.Itens.Add(trofeuCase);
            salaEstar.Itens.Add(tapete);


            // --- PORÃO (CELLAR) ---
            var porao = new Sala("cellar", "Porão",
                "Você está em um porão escuro e úmido. Uma passagem estreita leva para o leste, e você pode subir de volta pela escada.");
            // IMPORTANTE: Sem flag Iluminada. Só se vê com lanterna.
            porao.Flags = FlagsSala.Interior;


            // --- FLORESTA 1 ---
            var floresta1 = new Sala("forest_1", "Floresta",
                "Esta é uma floresta, com árvores em todas as direções. A leste, parece haver luz do sol.");
            floresta1.Flags = FlagsSala.Iluminada;

            // --- FLORESTA 2 ---
            var floresta2 = new Sala("forest_2", "Floresta",
                "Esta é uma floresta pouco iluminada, com grandes árvores por toda parte. Uma árvore particularmente grande com alguns galhos baixos está aqui.");
            floresta2.Flags = FlagsSala.Iluminada;

            // --- FLORESTA 3 ---
            var floresta3 = new Sala("forest_3", "Floresta",
                "Esta é uma floresta pouco iluminada, com grandes árvores por toda parte.");
            floresta3.Flags = FlagsSala.Iluminada;

            // --- CLAREIRA COM GRADE (GRATING-CLEARING) ---
            var clareia = new Sala("grating_clearing", "Clareira",
                "Você está em uma clareira, com uma floresta cercando você por todos os lados. Um caminho leva ao sul. " +
                "No chão há uma grade presa com um cadeado.");
            clareia.Flags = FlagsSala.Iluminada;

            // --- MONTE DE PEDRAS (STONE-BARROW) ---
            var pedreiraBarrow = new Sala("stone_barrow", "Monte de Pedras",
                "Você está de pé na frente de um monte maciço de pedras. Na face leste há uma enorme porta de pedra que está aberta. " +
                "Você não consegue ver dentro da escuridão da tumba.");
            pedreiraBarrow.Flags = FlagsSala.Iluminada;

            // --- SÓTÃO (ATTIC) ---
            var soton = new Sala("attic", "Sótão",
                "Este é o sótão. O único caminho de saída é por uma escadaria que desce.");
            soton.Flags = FlagsSala.Iluminada | FlagsSala.Interior;
            
            // Mesa do sótão com faca
            var mesaSoton = new Item("attic_table", "Mesa do sótão", "Uma mesa velha e empoeirada.",
                FlagsItem.Cenario | FlagsItem.Container | FlagsItem.EstaAberto);
            mesaSoton.AdicionarSinonimo("mesa", "table");
            
            var faca = new Item("knife", "Faca afiada", "Uma faca desagradável e afiada.",
                FlagsItem.Pegavel | FlagsItem.Arma | FlagsItem.Ferramenta);
            faca.AdicionarSinonimo("knife", "faca", "nasty");
            mesaSoton.Conteudo.Add(faca);
            
            soton.Itens.Add(mesaSoton);

            // --- ESTÚDIO (STUDIO) ---
            var estudio = new Sala("studio", "Estúdio",
                "Este parece ser um estúdio de artista. Uma escada sobe, e há uma saída ao sul.");
            estudio.Flags = FlagsSala.Iluminada | FlagsSala.Interior;
            
            // Pintura no estúdio
            var pintura = new Item("painting", "Pintura", "Uma pintura abstrata sem valor aparente.",
                FlagsItem.Pegavel);
            pintura.AdicionarSinonimo("painting", "pintura", "art");
            estudio.Itens.Add(pintura);

            // --- GALERIA (GALLERY) ---
            var galeria = new Sala("gallery", "Galeria",
                "Esta é uma galeria de arte. Saídas levam ao norte e oeste.");
            galeria.Flags = FlagsSala.Iluminada | FlagsSala.Interior;

            // --- CLAREIRA (CLEARING) ---
            var clearing = new Sala("clearing", "Clareira",
                "Você está em uma pequena clareira em um caminho de floresta bem marcado que se estende para leste e oeste.");
            clearing.Flags = FlagsSala.Iluminada;

            // --- CAMINHO DA FLORESTA (PATH) ---
            var caminhoFloresta = new Sala("forest_path", "Caminho da Floresta",
                "Este é um caminho serpenteando através de uma floresta pouco iluminada. O caminho segue norte-sul aqui. " +
                "Uma árvore particularmente grande com alguns galhos baixos está na beirada do caminho.");
            caminhoFloresta.Flags = FlagsSala.Iluminada;

            var arvore = new Item("tree", "Árvore grande", "Uma árvore imensa e antiga.", FlagsItem.Cenario);
            arvore.AdicionarSinonimo("arvore");
            caminhoFloresta.Itens.Add(arvore);


            // --- EM CIMA DA ÁRVORE ---
            var cimaArvore = new Sala("up_tree", "Em cima da Árvore",
                "Você está a cerca de 3 metros do chão, aninhado entre alguns grandes galhos. " +
                "O galho mais próximo acima de você está fora de alcance.");
            cimaArvore.Flags = FlagsSala.Iluminada;

            var ninho = new Item("nest", "Ninho de pássaro", "Um pequeno ninho de pássaro.", 
                FlagsItem.Container | FlagsItem.Pegavel | FlagsItem.PodeAbrir | FlagsItem.EstaAberto);
            ninho.DescricaoNoChao = "Ao seu lado no galho há um pequeno ninho de pássaro.";
            ninho.AdicionarSinonimo("nest", "ninho");

            var ovo = new Item("egg", "Ovo com joias", 
                "Um grande ovo incrustado com joias preciosas.", 
                FlagsItem.Pegavel | FlagsItem.Container | FlagsItem.PodeAbrir);
            ovo.AdicionarSinonimo("ovo", "joia", "egg");
            ovo.DescricaoNoChao = "No ninho de pássaro há um grande ovo incrustado com joias preciosas, " +
                "aparentemente coletado por um pássaro canoro sem filhos. O ovo é coberto com fina incrustação de ouro, " +
                "e ornamentado em lápis-lazúli e madrepérola. Ao contrário da maioria dos ovos, este é articulado e " +
                "fechado com um fecho de aparência delicada. O ovo parece extremamente frágil.";
            
            var canario = new Item("canary", "Canário de corda dourado", 
                "Há um canário de corda dourado aninhado no ovo. Ele tem olhos de rubi e um bico de prata. " +
                "Através de uma janela de cristal abaixo de sua asa esquerda, você pode ver uma maquinaria " +
                "intrincada dentro. Parece ter parado de funcionar.",
                FlagsItem.Pegavel);
            canario.AdicionarSinonimo("canario", "canário", "passaro", "pássaro", "bird");
            ovo.Conteudo.Add(canario);
            
            // ZIL: BROKEN-EGG (quando danificado)
            var ovoQuebrado = new Item("broken_egg", "Ovo quebrado com joias", 
                "O ovo agora está aberto, mas a falta de jeito de sua tentativa comprometeu seriamente seu apelo estético.",
                FlagsItem.Pegavel | FlagsItem.Container | FlagsItem.EstaAberto);
            ovoQuebrado.AdicionarSinonimo("ovo", "egg", "broken");
            
            // ZIL: BROKEN-CANARY (quando danificado)
            var canarioQuebrado = new Item("broken_canary", "Canário de corda quebrado",
                "Há um canário de corda dourado aninhado no ovo. Ele parece ter tido recentemente uma experiência ruim. " +
                "As montagens para seus olhos como joias estão vazias, e seu bico de prata está amassado. " +
                "Através de uma janela de cristal rachada abaixo de sua asa esquerda, você pode ver os restos " +
                "de uma maquinaria intrincada. Pode ter algum valor como curiosidade.",
                FlagsItem.Pegavel);
            canarioQuebrado.AdicionarSinonimo("canario", "canário", "passaro", "pássaro", "bird", "broken");
            ovoQuebrado.Conteudo.Add(canarioQuebrado);
            
            ninho.Conteudo.Add(ovo);
            
            cimaArvore.Itens.Add(ninho);


            // =========================================================================
            // 3. DEFINIÇÃO DAS CONEXÕES (SAÍDAS) - BASEADO NO JSON ORIGINAL
            // =========================================================================

            // WEST-OF-HOUSE
            oesteCasa.DefinirSaida(Direcao.Norte, "north_house");
            oesteCasa.DefinirSaida(Direcao.Sul, "south_house");
            oesteCasa.DefinirSaida(Direcao.Oeste, "forest_1");
            // NE, SE, SW e IN ainda não implementados (precisam de direções diagonais)

            // NORTH-OF-HOUSE
            norteCasa.DefinirSaida(Direcao.Oeste, "west_house");
            norteCasa.DefinirSaida(Direcao.Leste, "east_house");
            norteCasa.DefinirSaida(Direcao.Norte, "forest_path");
            // SW, SE ainda não implementados (precisam de direções diagonais)

            // SOUTH-OF-HOUSE
            sulCasa.DefinirSaida(Direcao.Oeste, "west_house");
            sulCasa.DefinirSaida(Direcao.Leste, "east_house");
            sulCasa.DefinirSaida(Direcao.Sul, "forest_3");
            // NE, NW ainda não implementados (precisam de direções diagonais)

            // EAST-OF-HOUSE (Behind House)
            lesteCasa.DefinirSaida(Direcao.Norte, "north_house");
            lesteCasa.DefinirSaida(Direcao.Sul, "south_house");
            lesteCasa.DefinirSaida(Direcao.Leste, "clearing");
            // Janela para a cozinha (Bloqueada até abrir a janela)
            lesteCasa.DefinirSaida(Direcao.Oeste, "kitchen", bloqueada: true, msgBloqueio: "A janela está fechada.");
            lesteCasa.DefinirSaida(Direcao.Entrar, "kitchen", bloqueada: true, msgBloqueio: "A janela está fechada.");
            // SW, NW ainda não implementados

            // CLEARING
            clearing.DefinirSaida(Direcao.Norte, "forest_2");
            clearing.DefinirSaida(Direcao.Sul, "forest_3");
            clearing.DefinirSaida(Direcao.Oeste, "east_house");
            // clearing.DefinirSaida(Direcao.Leste, "canyon_view"); // Futuro

            // KITCHEN
            cozinha.DefinirSaida(Direcao.Leste, "east_house"); 
            cozinha.DefinirSaida(Direcao.Sair, "east_house");
            cozinha.DefinirSaida(Direcao.Oeste, "living_room");
            cozinha.DefinirSaida(Direcao.Cima, "attic");
            cozinha.DefinirSaida(Direcao.Baixo, "studio");

            // ATTIC
            soton.DefinirSaida(Direcao.Baixo, "kitchen");

            // STUDIO
            estudio.DefinirSaida(Direcao.Sul, "gallery");
            estudio.DefinirSaida(Direcao.Cima, "kitchen");

            // GALLERY
            galeria.DefinirSaida(Direcao.Norte, "studio");
            // galeria.DefinirSaida(Direcao.Oeste, "east_of_chasm"); // Futuro

            // LIVING-ROOM
            salaEstar.DefinirSaida(Direcao.Leste, "kitchen");
            // salaEstar.DefinirSaida(Direcao.Oeste, "strange_passage"); // Futuro
            // Alçapão para o porão (Bloqueada até mover tapete e abrir alçapão)
            // salaEstar.DefinirSaida(Direcao.Baixo, "cellar", bloqueada: true, msgBloqueio: "Você não consegue atravessar o chão.");

            // CELLAR
            // porao.DefinirSaida(Direcao.Cima, "living_room", bloqueada: true, msgBloqueio: "O alçapão está fechado.");
            // porao.DefinirSaida(Direcao.Norte, "troll_room"); // Futuro
            // porao.DefinirSaida(Direcao.Sul, "east_of_chasm"); // Futuro

            // FOREST-PATH
            caminhoFloresta.DefinirSaida(Direcao.Cima, "up_tree");
            caminhoFloresta.DefinirSaida(Direcao.Norte, "grating_clearing");
            caminhoFloresta.DefinirSaida(Direcao.Leste, "forest_2");
            caminhoFloresta.DefinirSaida(Direcao.Sul, "north_house");
            caminhoFloresta.DefinirSaida(Direcao.Oeste, "forest_1");

            // UP-A-TREE
            cimaArvore.DefinirSaida(Direcao.Baixo, "forest_path");

            // FOREST-1
            floresta1.DefinirSaida(Direcao.Norte, "grating_clearing");
            floresta1.DefinirSaida(Direcao.Leste, "forest_path");
            floresta1.DefinirSaida(Direcao.Sul, "forest_3");

            // FOREST-2
            floresta2.DefinirSaida(Direcao.Sul, "clearing");
            floresta2.DefinirSaida(Direcao.Oeste, "forest_path");
            // floresta2.DefinirSaida(Direcao.Leste, "mountains"); // Futuro

            // FOREST-3
            floresta3.DefinirSaida(Direcao.Norte, "clearing");
            floresta3.DefinirSaida(Direcao.Oeste, "forest_1");
            // floresta3.DefinirSaida(Direcao.NW, "south_house"); // Requer direção diagonal

            // GRATING-CLEARING
            clareia.DefinirSaida(Direcao.Leste, "forest_2");
            clareia.DefinirSaida(Direcao.Oeste, "forest_1");
            clareia.DefinirSaida(Direcao.Sul, "forest_path");

            // STONE-BARROW
            // pedreiraBarrow.DefinirSaida(Direcao.NE, "west_house"); // Requer direção diagonal

            // CLEARING (conecta com EAST-OF-HOUSE)
            // Nota: CLEARING ainda não foi criada como sala, será necessário criar depois


            // =========================================================================
            // 4. PREENCHIMENTO DO DICIONÁRIO
            // =========================================================================
            mundo.Add(oesteCasa.Id, oesteCasa);
            mundo.Add(norteCasa.Id, norteCasa);
            mundo.Add(sulCasa.Id, sulCasa);
            mundo.Add(lesteCasa.Id, lesteCasa);
            mundo.Add(cozinha.Id, cozinha);
            mundo.Add(salaEstar.Id, salaEstar);
            mundo.Add(porao.Id, porao);
            mundo.Add(soton.Id, soton);
            mundo.Add(estudio.Id, estudio);
            mundo.Add(galeria.Id, galeria);
            mundo.Add(floresta1.Id, floresta1);
            mundo.Add(floresta2.Id, floresta2);
            mundo.Add(floresta3.Id, floresta3);
            mundo.Add(clareia.Id, clareia);
            mundo.Add(pedreiraBarrow.Id, pedreiraBarrow);
            mundo.Add(clearing.Id, clearing);
            mundo.Add(caminhoFloresta.Id, caminhoFloresta);
            mundo.Add(cimaArvore.Id, cimaArvore);

            return mundo;
        }
    }
}