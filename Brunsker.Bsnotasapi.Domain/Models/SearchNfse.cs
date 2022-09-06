using System;
using static Brunsker.AutoMapperProcedure.CustomAttributes.AttributeNameProcedure;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class SearchNfse
    {
        [NameParamProcedure("pSEQ_CLIENTE")]
        public int SeqCliente { get; set; }
        [NameParamProcedure("pDATAINI")]
        public DateTime DataInicial { get; set; }
        [NameParamProcedure("pDATAFIM")]
        public DateTime DataFinal { get; set; }
    }
}
