using static Brunsker.AutoMapperProcedure.CustomAttributes.AttributeNameProcedure;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class SearchCompany
    {
        [NameParamProcedure("pSEQ_CLIENTE")]
        public int ClientNumber { get; set; }
    }
}
