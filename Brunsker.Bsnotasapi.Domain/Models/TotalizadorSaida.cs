namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class TotalizadorSaida
    {
        public double? VLTOTAL { get; set; }
        public long QTNFCANCELADAS { get; set; }
        public long QTNFERP { get; set; }
        public long QTNFDENEGADAS { get; set; }
        public long QTNFDEVOLUCAO { get; set; }
        public long QTNFTRANSFERENCIA { get; set; }
    }
}