using System;
using System.Windows.Forms;

using Compilador.Lexico;
using Compilador.Sintatico.Ascendente.SLR;

namespace Compilador
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            // Limpa a lista de tokens e do resultado sintático.
            this.lstTokens.Items.Clear();
            this.lstSintatico.Items.Clear();
            
            // Declara o analisador léxico.
            LexicoClass lexico;

            try
            {
                // Tenta criar uma instância do analisador léxico
                lexico = new LexicoClass(this.rtxtCode.Text, marcadorFinal: "$");

                // Adiciona à lista os novos tokens encontrados, se existirem:
                foreach (var token in lexico.ListaTokens)
                    this.lstTokens.Items.Add(token);
                
                // Executa a análise sintática usando a lista de tokens encontrados
                AscSLR.Verificar(lexico, this.lstSintatico);
            }
            catch (Exception ex)
            {
                this.lstSintatico.Items.Add("--> Erro sintático <---");
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Seleciona o último item da lista de resultados do processamento sintático:
            if (this.lstSintatico.Items.Count > 0)
                this.lstSintatico.SelectedIndex = this.lstSintatico.Items.Count - 1;
        }
    }
}
