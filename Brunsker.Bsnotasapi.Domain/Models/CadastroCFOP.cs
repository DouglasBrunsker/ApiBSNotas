using System;
using System.Collections.Generic;
using System.Text;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class CadastroCFOP
    {
        public int SEQ_CLIENTE { get; set; }
        public string STRING_BANCO { get; set; }
        public int CFOPENT { get; set; }
        public int CFOPSAIDA { get; set; }
        public string DESCRICAO { get; set; }
    }
}
