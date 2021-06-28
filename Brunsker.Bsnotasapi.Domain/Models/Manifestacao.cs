namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Manifestacao
    {
        public string Chave { get; set; }
        public string CnpjEmitente { get; set; }
        public string CnpjDestinatario { get; set; }
        public string Codigo { get; set; }
        public string Justificativa { get; set; }
        public int SeqCliente { get; set; }
    }
}