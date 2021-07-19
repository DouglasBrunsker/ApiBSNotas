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
        public int Index { get; set; }
        public int Length { get; set; }
        public bool Devolucao { get; set; }
        public bool Transferencia { get; set; }
        public bool Autorizadas { get; set; }
        public bool Canceladas { get; set; }
        public bool Denegadas { get; set; }
    }
}