using System;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class PesquisaNfEntrada
    {
        public long SeqCliente { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
