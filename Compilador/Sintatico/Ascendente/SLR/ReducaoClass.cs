using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Sintatico.Ascendente.SLR
{
    /// <summary>
    /// Classe de armazenamento de uma redução do analisador sintático SLR.
    /// </summary>
    public class ReducaoClass
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="to">Simbolo não terminal da redução.</param>
        /// <param name="from">Vetor com os simbolos a serem reduzidos (desempilhados).</param>
        public ReducaoClass(string to, string[] from)
        {
            this.To = to;
            this.From = from;
        }

        /// <summary>
        /// Obtém ou define o simbolo não terminal da redução.
        /// </summary>
        public string[] From { get; set; }

        /// <summary>
        /// Obtém ou define o vetor com os simbolos a serem reduzidos (desempilhados).
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Retorna um texto contendo as informações do token armazenado.
        /// </summary>
        public override string ToString()
        {
            string from = String.Empty;
            foreach (var item in this.From)
                from += $"{item} ";

            if (String.IsNullOrEmpty(from))
                from = "ε";

            return $"{this.To} --> {from.Trim()}";
        }
    }
}
