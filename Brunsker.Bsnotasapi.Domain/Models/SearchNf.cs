using static Brunsker.AutoMapperProcedure.CustomAttributes.AttributeNameProcedure;
using System;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class SearchNf
    {
        [NameParamProcedure("pSEQ_CLIENTE")]
        public int SeqCliente { get; set; }
        [NameParamProcedure("pDATAINI")]
        public DateTime DataInicial { get; set; }
        [NameParamProcedure("pDATAFIM")]
        public DateTime DataFinal { get; set; }
        [NameParamProcedure("pPRESTADOR_CNPJ")]
        public string? PrestadorCnpj { get; set; }
        [NameParamProcedure("pTOMADOR_CNPJ")]
        public string? TomadorCnpj { get; set; }
        [NameParamProcedure("pNUMERO_RPS")]
        public int? NumeroRps { get; set; }
        [NameParamProcedure("pPRESTADOR_RAZAO_SOCIAL")]
        public string? PrestadorRazaoSocial { get; set; }
        [NameParamProcedure("pTOMADOR_RAZAO_SOCIAL")]
        public string? TomadorRazaoSocial { get; set; }
        [NameParamProcedure("pSTATUSNORMAL")]
        public int? StatusNormal { get; set; }
        [NameParamProcedure("pSTATUSCANCELADA")]
        public int? StatusCancelada { get; set; }
        [NameParamProcedure("pSTATUSEXTRAVIADA")]
        public int? StatusExtraviada { get; set; }
        [NameParamProcedure("pSTATUSLOTE")]
        public int? StatusLote { get; set; }
        [NameParamProcedure("pEMPRESASCADASTRADAS")]
        public string? EmpresasCadastradas { get; set; }
    }
}
