namespace Brunsker.Bsnotas.Application.Responses.Pdf
{
    public sealed class PdfResponse
    {
        public int SeqArquivoXmlNfse { get; set; }
        public byte[] ArquivoPdf { get; set; }
        public string NomePdf { get; set; }
    }
}
