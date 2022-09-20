using static Brunsker.AutoMapperProcedure.CustomAttributes.AttributeNameProcedure;

namespace Brunsker.Bsnotas.Domain.Models
{
    public sealed class GeneratePdf
    {
        [NameParamProcedure("pSEQ_CLIENTE")]
        public int SeqCliente { get; set; }

        [NameParamProcedure("pSEQ_ARQUIVOXML_NFSE")]
        public int SeqArquivoXmlNfse { get; set; }
    }
}
