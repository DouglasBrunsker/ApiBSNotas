namespace Brunsker.Bsnotas.Application.Requests.GeneratePdf
{
    public sealed class GeneratePdfRequest
    {
        public int SeqCliente { get; set; }
        public int SeqArquivoXmlNfse { get; set; }
    }
}
