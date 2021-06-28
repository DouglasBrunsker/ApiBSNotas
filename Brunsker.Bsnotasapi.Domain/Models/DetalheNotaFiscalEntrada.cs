namespace Brunsker.Bsnotas.Domain.Models
{
    public class DetalheNotaFiscalEntrada
    {
        public string CARTACORRECAONFE { get; set; }
        public string ARQUIVO_XML { get; set; }
        public string CNPJ_EMITENTE { get; set; }
        public string EMITENTE { get; set; }
        public string CNPJ_DESTINATARIO { get; set; }
        public string DESTINATARIO { get; set; }
        public string NUMTRANSACAO { get; set; }
        public string CFOP { get; set; }
        public string NUMPED { get; set; }
        public decimal VALORNF { get; set; }
        public string TIPONOTA { get; set; }
    }
}
