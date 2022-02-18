using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Cte
    {
        public string SITUACAO_CTE { get; set; }
        public string CHAVE { get; set; }
        public long CODIGO_TP_CTE { get; set; }
        public long CODIGO_TOMADOR { get; set; }
        public long SEQ_EMITENTE_CTE { get; set; }
        public long NUMERO_CTE { get; set; }
        public DateTime? DT_RECEBIMENTO { get; set; }
        public DateTime? DT_LEITURA { get; set; }
        public string ARQUIVO_XML { get; set; }
        public DateTime? DT_VALIDACAO { get; set; }
        public string CNPJ_EMITENTE { get; set; }
        public string EMITENTE{ get; set; }
        public long CODUF_CTE { get; set; }
        public string DT_EMISSAO { get; set; }
        public string NAT_OP { get; set; }
        public decimal VALOR_TOTAL_CTE { get; set; }
        public decimal VALOR_RECEBER_CTE { get; set; }
        public long CFOP { get; set; }
        public long SEQ_MANIFESTACAO_CTE { get; set; }
        public string TOMADOR { get; set; }
        public string CNPJ_TOMADOR { get; set; }
        public string PROTOCOLO_MANIFESTACAO { get; set; }
    }
}