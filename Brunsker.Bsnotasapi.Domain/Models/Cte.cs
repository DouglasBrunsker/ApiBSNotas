using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Cte
    {
        public int SEQ_SITUACAO_CTE { get; set; }
        public string CHAVE { get; set; }
        public int CODIGO_TP_CTE { get; set; }
        public int CODIGO_TOMADOR { get; set; }
        public int SEQ_EMITENTE_CTE { get; set; }
        public int NUMERO_CTE { get; set; }
        public DateTime? DT_RECEBIMENTO { get; set; }
        public DateTime? DT_LEITURA { get; set; }
        public int SEQ_ARQUIVOXML_CTE { get; set; }
        public DateTime? DT_VALIDACAO { get; set; }
        public string CNPJ_EMITENTE { get; set; }
        public string EMITENTE{ get; set; }
        public int CODUF_CTE { get; set; }
        public string DT_EMISSAO { get; set; }
        public string NAT_OP { get; set; }
        public string VALOR_TOTAL_CTE { get; set; }
        public string VALOR_RECEBER_CTE { get; set; }
        public string CFOP { get; set; }
        public int SEQ_MANIFESTACAO_CTE { get; set; }
        public string TOMADOR { get; set; }
        public string CNPJ_TOMADOR { get; set; }
    }
}