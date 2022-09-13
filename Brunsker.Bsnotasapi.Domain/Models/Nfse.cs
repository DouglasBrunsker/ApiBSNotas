using System;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class Nfse
    {
        public int SEQ_ARQUIVOXML_NFSE { get; set; }
        public string APELIDO { get; set; }
        public string ARQUIVO_XML { get; set; }
        public string PRESTADOR_RAZAO_SOCIAL { get; set; }
        public decimal NUMERO_RPS { get; set; }
        public decimal VALOR_SERVICO { get; set; }
        public string TOMADOR_RAZAO_SOCIAL { get; set; }
        public string OPT_SIMPLES_NACIONAL { get; set; }
        public string ISS_RETIDO { get; set; }
        public string STATUS { get; set; }
        public DateTime DT_EMISSAO { get; set; }
        public string PRESTADOR_CNPJ { get; set; }
        public decimal VALOR_ISS_RETIDO { get; set; }
        public string OBS { get; set; }
        public string DESCRIMINACAO { get; set; }
    }
}
