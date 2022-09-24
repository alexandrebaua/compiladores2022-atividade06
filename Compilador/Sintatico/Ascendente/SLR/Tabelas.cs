using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Sintatico.Ascendente.SLR
{
    /// <summary>
    /// Tabelas de ações e desvio (GOTO) do analisador sintatico ascendente SLR.
    /// </summary>
    public static class Tabelas
    {
        private static Dictionary<string, ActionClass>[] tabelaAcao = null;
        private static Dictionary<string, int>[] tabelaGOTO = null;

        /// <summary>
        /// Obtém a tabela de ação.
        /// </summary>
        public static Dictionary<string, ActionClass>[] TabelaAcao
        {
            get
            {
                if (Tabelas.tabelaAcao == null)
                    Tabelas.InitTableAction();

                return Tabelas.tabelaAcao;
            }
        }

        /// <summary>
        /// Obtém a tabela de desvio (GOTO).
        /// </summary>
        public static Dictionary<string, int>[] TabelaGOTO
        {
            get
            {
                if (Tabelas.tabelaGOTO == null)
                    Tabelas.InitTableGOTO();

                return Tabelas.tabelaGOTO;
            }
        }

        /// <summary>
        /// Busca uma ação associada à tupla informada.
        /// </summary>
        /// <param name="s">O estado no topo da pilha.</param>
        /// <param name="a">O simbolo atual.</param>
        /// <returns>A ação encontrada, ou nulo se não existir nenhuma ação associada à tupla.</returns>
        public static ActionClass BuscaAcao(int s, string a)
        {
            if (Tabelas.TabelaAcao[s].ContainsKey(a))
                return Tabelas.TabelaAcao[s][a];

            return null;
        }

        /// <summary>
        /// Busca um desvio associado à tupla informada.
        /// </summary>
        /// <param name="t">O estado no topo da pilha.</param>
        /// <param name="A">O simbolo não terminal.</param>
        /// <returns>O devio encontrado, ou -1 se não existir nenhuma ação associada à tupla.</returns>
        public static int BuscaGOTO(int t, string A)
        {
            if (Tabelas.TabelaGOTO[t].ContainsKey(A))
                return Tabelas.TabelaGOTO[t][A];

            return -1;
        }

        /// <summary>
        /// Inicializa a tabela de ação.
        /// </summary>
        private static void InitTableAction()
        {
            // Define localmente todas as reduções da gramática, para serem utilizadas durante a criação dos estados:
            ReducaoClass r1 = new ReducaoClass("<FUNCTION>", new string[] { "FUNCTION", "ID", "ABRE_PAR", "<FUNC_BLOCO_PARAMETRO>", "FECHA_PAR", "ABRE_BLOCO", "<LISTA_BLOCO>", "FECHA_BLOCO" });
            ReducaoClass r2 = new ReducaoClass("<FUNC_PARAMETRO>", new string[] { "INT", "ID" });
            ReducaoClass r3 = new ReducaoClass("<FUNC_LISTA_PARAMETRO>", new string[] { "<FUNC_PARAMETRO>" });
            ReducaoClass r4 = new ReducaoClass("<FUNC_BLOCO_PARAMETRO>", new string[] { "<FUNC_LISTA_PARAMETRO>" });
            ReducaoClass r5 = new ReducaoClass("<FUNC_LISTA_PARAMETRO>", new string[] { "<FUNC_LISTA_PARAMETRO>", "VIRGULA", "<FUNC_PARAMETRO>" });
            ReducaoClass r6 = new ReducaoClass("<DECLARA_VAR>", new string[] { "INT", "ID", "PONTO_VIRGULA" });
            ReducaoClass r7 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<DECLARA_VAR>" });
            ReducaoClass r8 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<LISTA_BLOCO>", "<DECLARA_VAR>" });
            ReducaoClass r9 = new ReducaoClass("<VAR>", new string[] { "ID | CONST" });
            ReducaoClass r10 = new ReducaoClass("<OPERACAO>", new string[] { "MENOS | MAIS | ASTERISTICO | BARRA_ESQUERDA" });
            ReducaoClass r11 = new ReducaoClass("<VAR>", new string[] { "<VAR>", "<OPERACAO>", "<VAR>" });
            ReducaoClass r12 = new ReducaoClass("<ATRIB>", new string[] { "ID", "ATRIBUICAO", "<VAR>", "PONTO_VIRGULA" });
            ReducaoClass r13 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<ATRIB>" });
            ReducaoClass r14 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<LISTA_BLOCO>", "<ATRIB>" });
            ReducaoClass r15 = new ReducaoClass("<IMPRIME>", new string[] { "PRINT", "ABRE_PAR", "ID", "FECHA_PAR", "PONTO_VIRGULA" });
            ReducaoClass r16 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<IMPRIME>" });
            ReducaoClass r17 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<LISTA_BLOCO>", "<IMPRIME>" });
            ReducaoClass r18 = new ReducaoClass("<SEL_IF>", new string[] { "IF", "ABRE_PAR", "<COMP_CONDICIONAL>", "FECHA_PAR", "ABRE_BLOCO", "<LISTA_BLOCO>", "FECHA_BLOCO" });
            ReducaoClass r19 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<SEL_IF>" });
            ReducaoClass r20 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<LISTA_BLOCO>", "<SEL_IF>" });
            ReducaoClass r21 = new ReducaoClass("<VAR_COMP>", new string[] { "ID | CONST" });
            ReducaoClass r22 = new ReducaoClass("<COMPARADOR>", new string[] { "MAIOR | MENOR | IGUALDADE | DIFERENTE" });
            ReducaoClass r23 = new ReducaoClass("<COMP_CONDICIONAL>", new string[] { "<VAR_COMP>", "<COMPARADOR>", "<VAR_COMP>" });
            ReducaoClass r24 = new ReducaoClass("<ENQUANTO>", new string[] { "WHILE", "ABRE_PAR", "<COMP_CONDICIONAL>", "FECHA_PAR", "ABRE_BLOCO", "<LISTA_BLOCO>", "FECHA_BLOCO" });
            ReducaoClass r25 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<ENQUANTO>" });
            ReducaoClass r26 = new ReducaoClass("<LISTA_BLOCO>", new string[] { "<LISTA_BLOCO>", "<ENQUANTO>" });
            ReducaoClass r27 = new ReducaoClass("<FUNC_BLOCO_PARAMETRO>", new string[] { });
            ReducaoClass r28 = new ReducaoClass("<LISTA_BLOCO>", new string[] { });

            // Criação dos estados da tabela de ação...
            Tabelas.tabelaAcao = new Dictionary<string, ActionClass>[61];

            Tabelas.tabelaAcao[0] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[0].Add("FUNCTION", new ActionClass(ActionType.Shift, 2));

            Tabelas.tabelaAcao[1] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[1].Add("$", new ActionClass(ActionType.Accept));

            Tabelas.tabelaAcao[2] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[2].Add("ID", new ActionClass(ActionType.Shift, 3));

            Tabelas.tabelaAcao[3] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[3].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 4));

            Tabelas.tabelaAcao[4] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[4].Add("INT", new ActionClass(ActionType.Shift, 5));
            Tabelas.tabelaAcao[4].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, 27, r27));

            Tabelas.tabelaAcao[5] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[5].Add("ID", new ActionClass(ActionType.Shift, 6));

            Tabelas.tabelaAcao[6] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[6].Add("VIRGULA", new ActionClass(ActionType.Reduce, 2, r2));
            Tabelas.tabelaAcao[6].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, 2, r2));

            Tabelas.tabelaAcao[7] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[7].Add("VIRGULA", new ActionClass(ActionType.Reduce, 3, r3));
            Tabelas.tabelaAcao[7].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, 3, r3));

            Tabelas.tabelaAcao[8] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[8].Add("VIRGULA", new ActionClass(ActionType.Shift, 9));
            Tabelas.tabelaAcao[8].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, 4, r4));

            Tabelas.tabelaAcao[9] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[9].Add("INT", new ActionClass(ActionType.Shift, 5));

            Tabelas.tabelaAcao[10] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[10].Add("VIRGULA", new ActionClass(ActionType.Reduce, 5, r5));
            Tabelas.tabelaAcao[10].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, 5, r5));

            Tabelas.tabelaAcao[11] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[11].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 12));

            Tabelas.tabelaAcao[12] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[12].Add("ABRE_BLOCO", new ActionClass(ActionType.Shift, 13));

            Tabelas.tabelaAcao[13] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[13].Add("INT", new ActionClass(ActionType.Shift, 16));
            Tabelas.tabelaAcao[13].Add("ID", new ActionClass(ActionType.Shift, 21));
            Tabelas.tabelaAcao[13].Add("PRINT", new ActionClass(ActionType.Shift, 31));
            Tabelas.tabelaAcao[13].Add("IF", new ActionClass(ActionType.Shift, 38));
            Tabelas.tabelaAcao[13].Add("WHILE", new ActionClass(ActionType.Shift, 52));
            Tabelas.tabelaAcao[13].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 28, r28));

            Tabelas.tabelaAcao[14] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[14].Add("INT", new ActionClass(ActionType.Shift, 16));
            Tabelas.tabelaAcao[14].Add("ID", new ActionClass(ActionType.Shift, 21));
            Tabelas.tabelaAcao[14].Add("PRINT", new ActionClass(ActionType.Shift, 31));
            Tabelas.tabelaAcao[14].Add("IF", new ActionClass(ActionType.Shift, 38));
            Tabelas.tabelaAcao[14].Add("WHILE", new ActionClass(ActionType.Shift, 52));
            Tabelas.tabelaAcao[14].Add("FECHA_BLOCO", new ActionClass(ActionType.Shift, 15));

            Tabelas.tabelaAcao[15] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[15].Add("$", new ActionClass(ActionType.Reduce, 1, r1));

            Tabelas.tabelaAcao[16] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[16].Add("ID", new ActionClass(ActionType.Shift, 17));

            Tabelas.tabelaAcao[17] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[17].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 18));

            Tabelas.tabelaAcao[18] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[18].Add("INT", new ActionClass(ActionType.Reduce, 6, r6));
            Tabelas.tabelaAcao[18].Add("ID", new ActionClass(ActionType.Reduce, 6, r6));
            Tabelas.tabelaAcao[18].Add("PRINT", new ActionClass(ActionType.Reduce, 6, r6));
            Tabelas.tabelaAcao[18].Add("IF", new ActionClass(ActionType.Reduce, 6, r6));
            Tabelas.tabelaAcao[18].Add("WHILE", new ActionClass(ActionType.Reduce, 6, r6));
            Tabelas.tabelaAcao[18].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 6, r6));

            Tabelas.tabelaAcao[19] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[19].Add("INT", new ActionClass(ActionType.Reduce, 7, r7));
            Tabelas.tabelaAcao[19].Add("ID", new ActionClass(ActionType.Reduce, 7, r7));
            Tabelas.tabelaAcao[19].Add("PRINT", new ActionClass(ActionType.Reduce, 7, r7));
            Tabelas.tabelaAcao[19].Add("IF", new ActionClass(ActionType.Reduce, 7, r7));
            Tabelas.tabelaAcao[19].Add("WHILE", new ActionClass(ActionType.Reduce, 7, r7));
            Tabelas.tabelaAcao[19].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 7, r7));

            Tabelas.tabelaAcao[20] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[20].Add("INT", new ActionClass(ActionType.Reduce, 8, r8));
            Tabelas.tabelaAcao[20].Add("ID", new ActionClass(ActionType.Reduce, 8, r8));
            Tabelas.tabelaAcao[20].Add("PRINT", new ActionClass(ActionType.Reduce, 8, r8));
            Tabelas.tabelaAcao[20].Add("IF", new ActionClass(ActionType.Reduce, 8, r8));
            Tabelas.tabelaAcao[20].Add("WHILE", new ActionClass(ActionType.Reduce, 8, r8));
            Tabelas.tabelaAcao[20].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 8, r8));

            Tabelas.tabelaAcao[21] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[21].Add("ATRIBUICAO", new ActionClass(ActionType.Shift, 22));

            Tabelas.tabelaAcao[22] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[22].Add("ID", new ActionClass(ActionType.Shift, 27));
            Tabelas.tabelaAcao[22].Add("CONST", new ActionClass(ActionType.Shift, 27));

            Tabelas.tabelaAcao[23] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[23].Add("MENOS", new ActionClass(ActionType.Shift, 28));
            Tabelas.tabelaAcao[23].Add("MAIS", new ActionClass(ActionType.Shift, 28));
            Tabelas.tabelaAcao[23].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 28));
            Tabelas.tabelaAcao[23].Add("BARRA_ESQUERDA", new ActionClass(ActionType.Shift, 28));
            Tabelas.tabelaAcao[23].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 24));

            Tabelas.tabelaAcao[24] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[24].Add("INT", new ActionClass(ActionType.Reduce, 12, r12));
            Tabelas.tabelaAcao[24].Add("ID", new ActionClass(ActionType.Reduce, 12, r12));
            Tabelas.tabelaAcao[24].Add("PRINT", new ActionClass(ActionType.Reduce, 12, r12));
            Tabelas.tabelaAcao[24].Add("IF", new ActionClass(ActionType.Reduce, 12, r12));
            Tabelas.tabelaAcao[24].Add("WHILE", new ActionClass(ActionType.Reduce, 12, r12));
            Tabelas.tabelaAcao[24].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 12, r12));

            Tabelas.tabelaAcao[25] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[25].Add("ID", new ActionClass(ActionType.Shift, 27));
            Tabelas.tabelaAcao[25].Add("CONST", new ActionClass(ActionType.Shift, 27));

            Tabelas.tabelaAcao[26] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[26].Add("MENOS", new ActionClass(ActionType.Reduce, 11, r11));
            Tabelas.tabelaAcao[26].Add("MAIS", new ActionClass(ActionType.Reduce, 11, r11));
            Tabelas.tabelaAcao[26].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, 11, r11));
            Tabelas.tabelaAcao[26].Add("BARRA_ESQUERDA", new ActionClass(ActionType.Reduce, 11, r11));
            Tabelas.tabelaAcao[26].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, 11, r11));

            Tabelas.tabelaAcao[27] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[27].Add("MENOS", new ActionClass(ActionType.Reduce, 9, r9));
            Tabelas.tabelaAcao[27].Add("MAIS", new ActionClass(ActionType.Reduce, 9, r9));
            Tabelas.tabelaAcao[27].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, 9, r9));
            Tabelas.tabelaAcao[27].Add("BARRA_ESQUERDA", new ActionClass(ActionType.Reduce, 9, r9));
            Tabelas.tabelaAcao[27].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, 9, r9));

            Tabelas.tabelaAcao[28] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[28].Add("ID", new ActionClass(ActionType.Reduce, 10, r10));
            Tabelas.tabelaAcao[28].Add("CONST", new ActionClass(ActionType.Reduce, 10, r10));

            Tabelas.tabelaAcao[29] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[29].Add("INT", new ActionClass(ActionType.Reduce, 13, r13));
            Tabelas.tabelaAcao[29].Add("ID", new ActionClass(ActionType.Reduce, 13, r13));
            Tabelas.tabelaAcao[29].Add("PRINT", new ActionClass(ActionType.Reduce, 13, r13));
            Tabelas.tabelaAcao[29].Add("IF", new ActionClass(ActionType.Reduce, 13, r13));
            Tabelas.tabelaAcao[29].Add("WHILE", new ActionClass(ActionType.Reduce, 13, r13));
            Tabelas.tabelaAcao[29].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 13, r13));

            Tabelas.tabelaAcao[30] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[30].Add("INT", new ActionClass(ActionType.Reduce, 14, r14));
            Tabelas.tabelaAcao[30].Add("ID", new ActionClass(ActionType.Reduce, 14, r14));
            Tabelas.tabelaAcao[30].Add("PRINT", new ActionClass(ActionType.Reduce, 14, r14));
            Tabelas.tabelaAcao[30].Add("IF", new ActionClass(ActionType.Reduce, 14, r14));
            Tabelas.tabelaAcao[30].Add("WHILE", new ActionClass(ActionType.Reduce, 14, r14));
            Tabelas.tabelaAcao[30].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 14, r14));

            Tabelas.tabelaAcao[31] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[31].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 32));

            Tabelas.tabelaAcao[32] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[32].Add("ID", new ActionClass(ActionType.Shift, 33));

            Tabelas.tabelaAcao[33] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[33].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 34));

            Tabelas.tabelaAcao[34] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[34].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 35));

            Tabelas.tabelaAcao[35] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[35].Add("INT", new ActionClass(ActionType.Reduce, 15, r15));
            Tabelas.tabelaAcao[35].Add("ID", new ActionClass(ActionType.Reduce, 15, r15));
            Tabelas.tabelaAcao[35].Add("PRINT", new ActionClass(ActionType.Reduce, 15, r15));
            Tabelas.tabelaAcao[35].Add("IF", new ActionClass(ActionType.Reduce, 15, r15));
            Tabelas.tabelaAcao[35].Add("WHILE", new ActionClass(ActionType.Reduce, 15, r15));
            Tabelas.tabelaAcao[35].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 15, r15));

            Tabelas.tabelaAcao[36] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[36].Add("INT", new ActionClass(ActionType.Reduce, 16, r16));
            Tabelas.tabelaAcao[36].Add("ID", new ActionClass(ActionType.Reduce, 16, r16));
            Tabelas.tabelaAcao[36].Add("PRINT", new ActionClass(ActionType.Reduce, 16, r16));
            Tabelas.tabelaAcao[36].Add("IF", new ActionClass(ActionType.Reduce, 16, r16));
            Tabelas.tabelaAcao[36].Add("WHILE", new ActionClass(ActionType.Reduce, 16, r16));
            Tabelas.tabelaAcao[36].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 16, r16));

            Tabelas.tabelaAcao[37] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[37].Add("INT", new ActionClass(ActionType.Reduce, 17, r17));
            Tabelas.tabelaAcao[37].Add("ID", new ActionClass(ActionType.Reduce, 17, r17));
            Tabelas.tabelaAcao[37].Add("PRINT", new ActionClass(ActionType.Reduce, 17, r17));
            Tabelas.tabelaAcao[37].Add("IF", new ActionClass(ActionType.Reduce, 17, r17));
            Tabelas.tabelaAcao[37].Add("WHILE", new ActionClass(ActionType.Reduce, 17, r17));
            Tabelas.tabelaAcao[37].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 17, r17));

            Tabelas.tabelaAcao[38] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[38].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 39));

            Tabelas.tabelaAcao[39] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[39].Add("ID", new ActionClass(ActionType.Shift, 50));
            Tabelas.tabelaAcao[39].Add("CONST", new ActionClass(ActionType.Shift, 50));

            Tabelas.tabelaAcao[40] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[40].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 41));

            Tabelas.tabelaAcao[41] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[41].Add("ABRE_BLOCO", new ActionClass(ActionType.Shift, 42));

            Tabelas.tabelaAcao[42] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[42].Add("INT", new ActionClass(ActionType.Shift, 16));
            Tabelas.tabelaAcao[42].Add("ID", new ActionClass(ActionType.Shift, 21));
            Tabelas.tabelaAcao[42].Add("PRINT", new ActionClass(ActionType.Shift, 31));
            Tabelas.tabelaAcao[42].Add("IF", new ActionClass(ActionType.Shift, 38));
            Tabelas.tabelaAcao[42].Add("WHILE", new ActionClass(ActionType.Shift, 52));
            Tabelas.tabelaAcao[42].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 28, r28));

            Tabelas.tabelaAcao[43] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[43].Add("INT", new ActionClass(ActionType.Shift, 16));
            Tabelas.tabelaAcao[43].Add("ID", new ActionClass(ActionType.Shift, 21));
            Tabelas.tabelaAcao[43].Add("PRINT", new ActionClass(ActionType.Shift, 31));
            Tabelas.tabelaAcao[43].Add("IF", new ActionClass(ActionType.Shift, 38));
            Tabelas.tabelaAcao[43].Add("WHILE", new ActionClass(ActionType.Shift, 52));
            Tabelas.tabelaAcao[43].Add("FECHA_BLOCO", new ActionClass(ActionType.Shift, 44));

            Tabelas.tabelaAcao[44] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[44].Add("INT", new ActionClass(ActionType.Reduce, 18, r18));
            Tabelas.tabelaAcao[44].Add("ID", new ActionClass(ActionType.Reduce, 18, r18));
            Tabelas.tabelaAcao[44].Add("PRINT", new ActionClass(ActionType.Reduce, 18, r18));
            Tabelas.tabelaAcao[44].Add("IF", new ActionClass(ActionType.Reduce, 18, r18));
            Tabelas.tabelaAcao[44].Add("WHILE", new ActionClass(ActionType.Reduce, 18, r18));
            Tabelas.tabelaAcao[44].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 18, r18));

            Tabelas.tabelaAcao[45] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[45].Add("INT", new ActionClass(ActionType.Reduce, 19, r19));
            Tabelas.tabelaAcao[45].Add("ID", new ActionClass(ActionType.Reduce, 19, r19));
            Tabelas.tabelaAcao[45].Add("PRINT", new ActionClass(ActionType.Reduce, 19, r19));
            Tabelas.tabelaAcao[45].Add("IF", new ActionClass(ActionType.Reduce, 19, r19));
            Tabelas.tabelaAcao[45].Add("WHILE", new ActionClass(ActionType.Reduce, 19, r19));
            Tabelas.tabelaAcao[45].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 19, r19));

            Tabelas.tabelaAcao[46] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[46].Add("INT", new ActionClass(ActionType.Reduce, 20, r20));
            Tabelas.tabelaAcao[46].Add("ID", new ActionClass(ActionType.Reduce, 20, r20));
            Tabelas.tabelaAcao[46].Add("PRINT", new ActionClass(ActionType.Reduce, 20, r20));
            Tabelas.tabelaAcao[46].Add("IF", new ActionClass(ActionType.Reduce, 20, r20));
            Tabelas.tabelaAcao[46].Add("WHILE", new ActionClass(ActionType.Reduce, 20, r20));
            Tabelas.tabelaAcao[46].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 20, r20));

            Tabelas.tabelaAcao[47] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[47].Add("MAIOR", new ActionClass(ActionType.Shift, 51));
            Tabelas.tabelaAcao[47].Add("MENOR", new ActionClass(ActionType.Shift, 51));
            Tabelas.tabelaAcao[47].Add("IGUALDADE", new ActionClass(ActionType.Shift, 51));
            Tabelas.tabelaAcao[47].Add("DIFERENTE", new ActionClass(ActionType.Shift, 51));

            Tabelas.tabelaAcao[48] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[48].Add("ID", new ActionClass(ActionType.Shift, 50));
            Tabelas.tabelaAcao[48].Add("CONST", new ActionClass(ActionType.Shift, 50));

            Tabelas.tabelaAcao[49] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[49].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, 23, r23));

            Tabelas.tabelaAcao[50] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[50].Add("MAIOR", new ActionClass(ActionType.Reduce, 21, r21));
            Tabelas.tabelaAcao[50].Add("MENOR", new ActionClass(ActionType.Reduce, 21, r21));
            Tabelas.tabelaAcao[50].Add("IGUALDADE", new ActionClass(ActionType.Reduce, 21, r21));
            Tabelas.tabelaAcao[50].Add("DIFERENTE", new ActionClass(ActionType.Reduce, 21, r21));
            Tabelas.tabelaAcao[50].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, 21, r21));

            Tabelas.tabelaAcao[51] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[51].Add("ID", new ActionClass(ActionType.Reduce, 22, r22));
            Tabelas.tabelaAcao[51].Add("CONST", new ActionClass(ActionType.Reduce, 22, r22));

            Tabelas.tabelaAcao[52] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[52].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 53));

            Tabelas.tabelaAcao[53] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[53].Add("ID", new ActionClass(ActionType.Shift, 50));
            Tabelas.tabelaAcao[53].Add("CONST", new ActionClass(ActionType.Shift, 50));

            Tabelas.tabelaAcao[54] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[54].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 55));

            Tabelas.tabelaAcao[55] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[55].Add("ABRE_BLOCO", new ActionClass(ActionType.Shift, 56));

            Tabelas.tabelaAcao[56] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[56].Add("INT", new ActionClass(ActionType.Shift, 16));
            Tabelas.tabelaAcao[56].Add("ID", new ActionClass(ActionType.Shift, 21));
            Tabelas.tabelaAcao[56].Add("PRINT", new ActionClass(ActionType.Shift, 31));
            Tabelas.tabelaAcao[56].Add("IF", new ActionClass(ActionType.Shift, 38));
            Tabelas.tabelaAcao[56].Add("WHILE", new ActionClass(ActionType.Shift, 52));
            Tabelas.tabelaAcao[56].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 28, r28));

            Tabelas.tabelaAcao[57] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[57].Add("INT", new ActionClass(ActionType.Shift, 16));
            Tabelas.tabelaAcao[57].Add("ID", new ActionClass(ActionType.Shift, 21));
            Tabelas.tabelaAcao[57].Add("PRINT", new ActionClass(ActionType.Shift, 31));
            Tabelas.tabelaAcao[57].Add("IF", new ActionClass(ActionType.Shift, 38));
            Tabelas.tabelaAcao[57].Add("WHILE", new ActionClass(ActionType.Shift, 52));
            Tabelas.tabelaAcao[57].Add("FECHA_BLOCO", new ActionClass(ActionType.Shift, 58));

            Tabelas.tabelaAcao[58] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[58].Add("INT", new ActionClass(ActionType.Reduce, 24, r24));
            Tabelas.tabelaAcao[58].Add("ID", new ActionClass(ActionType.Reduce, 24, r24));
            Tabelas.tabelaAcao[58].Add("PRINT", new ActionClass(ActionType.Reduce, 24, r24));
            Tabelas.tabelaAcao[58].Add("IF", new ActionClass(ActionType.Reduce, 24, r24));
            Tabelas.tabelaAcao[58].Add("WHILE", new ActionClass(ActionType.Reduce, 24, r24));
            Tabelas.tabelaAcao[58].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 24, r24));

            Tabelas.tabelaAcao[59] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[59].Add("INT", new ActionClass(ActionType.Reduce, 25, r25));
            Tabelas.tabelaAcao[59].Add("ID", new ActionClass(ActionType.Reduce, 25, r25));
            Tabelas.tabelaAcao[59].Add("PRINT", new ActionClass(ActionType.Reduce, 25, r25));
            Tabelas.tabelaAcao[59].Add("IF", new ActionClass(ActionType.Reduce, 25, r25));
            Tabelas.tabelaAcao[59].Add("WHILE", new ActionClass(ActionType.Reduce, 25, r25));
            Tabelas.tabelaAcao[59].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 25, r25));

            Tabelas.tabelaAcao[60] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[60].Add("INT", new ActionClass(ActionType.Reduce, 26, r26));
            Tabelas.tabelaAcao[60].Add("ID", new ActionClass(ActionType.Reduce, 26, r26));
            Tabelas.tabelaAcao[60].Add("PRINT", new ActionClass(ActionType.Reduce, 26, r26));
            Tabelas.tabelaAcao[60].Add("IF", new ActionClass(ActionType.Reduce, 26, r26));
            Tabelas.tabelaAcao[60].Add("WHILE", new ActionClass(ActionType.Reduce, 26, r26));
            Tabelas.tabelaAcao[60].Add("FECHA_BLOCO", new ActionClass(ActionType.Reduce, 26, r26));
        }

        /// <summary>
        /// Inicializa a tabela de desvio (GOTO).
        /// </summary>
        private static void InitTableGOTO()
        {
            Tabelas.tabelaGOTO = new Dictionary<string, int>[61];

            Tabelas.tabelaGOTO[0] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[0].Add("<FUNCTION>", 1);

            Tabelas.tabelaGOTO[1] = null;
            Tabelas.tabelaGOTO[2] = null;
            Tabelas.tabelaGOTO[3] = null;

            Tabelas.tabelaGOTO[4] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[4].Add("<FUNC_PARAMETRO>", 7);
            Tabelas.tabelaGOTO[4].Add("<FUNC_LISTA_PARAMETRO>", 8);
            Tabelas.tabelaGOTO[4].Add("<FUNC_BLOCO_PARAMETRO>", 11);

            Tabelas.tabelaGOTO[5] = null;
            Tabelas.tabelaGOTO[6] = null;
            Tabelas.tabelaGOTO[7] = null;
            Tabelas.tabelaGOTO[8] = null;

            Tabelas.tabelaGOTO[9] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[9].Add("<FUNC_PARAMETRO>", 10);

            Tabelas.tabelaGOTO[10] = null;
            Tabelas.tabelaGOTO[11] = null;
            Tabelas.tabelaGOTO[12] = null;

            Tabelas.tabelaGOTO[13] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[13].Add("<DECLARA_VAR>", 19);
            Tabelas.tabelaGOTO[13].Add("<ATRIB>", 29);
            Tabelas.tabelaGOTO[13].Add("<IMPRIME>", 36);
            Tabelas.tabelaGOTO[13].Add("<SEL_IF>", 45);
            Tabelas.tabelaGOTO[13].Add("<ENQUANTO>", 59);
            Tabelas.tabelaGOTO[13].Add("<LISTA_BLOCO>", 14);

            Tabelas.tabelaGOTO[14] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[14].Add("<DECLARA_VAR>", 20);
            Tabelas.tabelaGOTO[14].Add("<ATRIB>", 30);
            Tabelas.tabelaGOTO[14].Add("<IMPRIME>", 37);
            Tabelas.tabelaGOTO[14].Add("<SEL_IF>", 46);
            Tabelas.tabelaGOTO[14].Add("<ENQUANTO>", 60);

            Tabelas.tabelaGOTO[15] = null;
            Tabelas.tabelaGOTO[16] = null;
            Tabelas.tabelaGOTO[17] = null;
            Tabelas.tabelaGOTO[18] = null;
            Tabelas.tabelaGOTO[19] = null;
            Tabelas.tabelaGOTO[20] = null;
            Tabelas.tabelaGOTO[21] = null;

            Tabelas.tabelaGOTO[22] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[22].Add("<VAR>", 23);

            Tabelas.tabelaGOTO[23] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[23].Add("<VAR>", 22);
            Tabelas.tabelaGOTO[23].Add("<OPERACAO>", 25);

            Tabelas.tabelaGOTO[24] = null;

            Tabelas.tabelaGOTO[25] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[25].Add("<VAR>", 26);

            Tabelas.tabelaGOTO[26] = null;
            Tabelas.tabelaGOTO[27] = null;
            Tabelas.tabelaGOTO[28] = null;
            Tabelas.tabelaGOTO[29] = null;
            Tabelas.tabelaGOTO[30] = null;
            Tabelas.tabelaGOTO[31] = null;
            Tabelas.tabelaGOTO[32] = null;
            Tabelas.tabelaGOTO[33] = null;
            Tabelas.tabelaGOTO[34] = null;
            Tabelas.tabelaGOTO[35] = null;
            Tabelas.tabelaGOTO[36] = null;
            Tabelas.tabelaGOTO[37] = null;
            Tabelas.tabelaGOTO[38] = null;

            Tabelas.tabelaGOTO[39] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[39].Add("<VAR_COMP>", 47);
            Tabelas.tabelaGOTO[39].Add("<COMP_CONDICIONAL>", 40);

            Tabelas.tabelaGOTO[40] = null;
            Tabelas.tabelaGOTO[41] = null;

            Tabelas.tabelaGOTO[42] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[42].Add("<DECLARA_VAR>", 19);
            Tabelas.tabelaGOTO[42].Add("<ATRIB>", 29);
            Tabelas.tabelaGOTO[42].Add("<IMPRIME>", 36);
            Tabelas.tabelaGOTO[42].Add("<SEL_IF>", 45);
            Tabelas.tabelaGOTO[42].Add("<ENQUANTO>", 59);
            Tabelas.tabelaGOTO[42].Add("<LISTA_BLOCO>", 43);

            Tabelas.tabelaGOTO[43] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[43].Add("<DECLARA_VAR>", 20);
            Tabelas.tabelaGOTO[43].Add("<ATRIB>", 30);
            Tabelas.tabelaGOTO[43].Add("<IMPRIME>", 37);
            Tabelas.tabelaGOTO[43].Add("<SEL_IF>", 46);
            Tabelas.tabelaGOTO[43].Add("<ENQUANTO>", 60);

            Tabelas.tabelaGOTO[44] = null;
            Tabelas.tabelaGOTO[45] = null;
            Tabelas.tabelaGOTO[46] = null;

            Tabelas.tabelaGOTO[47] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[47].Add("<COMPARADOR>", 48);

            Tabelas.tabelaGOTO[48] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[48].Add("<VAR_COMP>", 49);

            Tabelas.tabelaGOTO[49] = null;
            Tabelas.tabelaGOTO[50] = null;
            Tabelas.tabelaGOTO[51] = null;
            Tabelas.tabelaGOTO[52] = null;

            Tabelas.tabelaGOTO[53] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[53].Add("<VAR_COMP>", 47);
            Tabelas.tabelaGOTO[53].Add("<COMP_CONDICIONAL>", 54);

            Tabelas.tabelaGOTO[54] = null;
            Tabelas.tabelaGOTO[55] = null;

            Tabelas.tabelaGOTO[56] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[56].Add("<DECLARA_VAR>", 19);
            Tabelas.tabelaGOTO[56].Add("<ATRIB>", 29);
            Tabelas.tabelaGOTO[56].Add("<IMPRIME>", 36);
            Tabelas.tabelaGOTO[56].Add("<SEL_IF>", 45);
            Tabelas.tabelaGOTO[56].Add("<ENQUANTO>", 59);
            Tabelas.tabelaGOTO[56].Add("<LISTA_BLOCO>", 57);

            Tabelas.tabelaGOTO[57] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[57].Add("<DECLARA_VAR>", 20);
            Tabelas.tabelaGOTO[57].Add("<ATRIB>", 30);
            Tabelas.tabelaGOTO[57].Add("<IMPRIME>", 37);
            Tabelas.tabelaGOTO[57].Add("<SEL_IF>", 46);
            Tabelas.tabelaGOTO[57].Add("<ENQUANTO>", 60);

            Tabelas.tabelaGOTO[58] = null;
            Tabelas.tabelaGOTO[59] = null;
            Tabelas.tabelaGOTO[60] = null;
        }
    }
}
