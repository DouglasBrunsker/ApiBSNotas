namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class TotalizadorSaida
    {
        public double? VLTOTAL { get; set; }
        public long QTNFCANCELADAS { get; set; }
        public long QTNF_ERP { get; set; }
        public long QTNFDENEGADAS { get; set; }
        public long QTNDDEVOLUCAO { get; set; }
    }
}