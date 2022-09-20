namespace Brunsker.Bsnotas.Domain.Models
{
    public sealed class Pdf
    {
        public int SEQ_ARQUIVOXML_NFSE { get; set; }
        //public string ARQUIVOPDF { get; set; }
        public byte[] ARQUIVOPDF { get; set; }
        public string NOMEPDF { get; set; }
    }
}
