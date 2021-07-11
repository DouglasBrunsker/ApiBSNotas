using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class FiltroBuscaNotasSaida
    {
        public int SeqCliente { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public string Chave { get; set; }
        public string NaturezaOperacao { get; set; }
        public long? NumeroNota { get; set; }
        public string CnpjEmitente { get; set; }
    }
}