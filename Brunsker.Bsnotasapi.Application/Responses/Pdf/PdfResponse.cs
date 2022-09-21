using System.IO;

namespace Brunsker.Bsnotas.Application.Responses.Pdf
{
    public sealed class PdfResponse
    {
        public MemoryStream ArquivoPdf { get; set; }
        public string NomePdf { get; set; }
    }
}
