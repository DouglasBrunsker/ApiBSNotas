using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class NF
    {
        public string APELIDO { get; set; }
        public long NUMNOTA { get; set; }
        public long CODCLI { get; set; }
        public string CLIENTE { get; set; }
        public long NUMTRANSVENDA { get; set; }
        public string CNPJ { get; set; }
        public string IE { get; set; }
        public int SERIE { get; set; }
        public string CGCFILIAL { get; set; }
        public long CODFORNECFRETE { get; set; }
        public DateTime DTSAIDA { get; set; }
        public DateTime? DTCANCEL { get; set; }
        public string ARQUIVO_XML { get; set; }
        public string CHAVE { get; set; }
        public string CNPJ_EMITENTE { get; set; }
        public string EMITENTE { get; set; }
        public string STATUSMANIF { get; set; }
        public string STATUSPREENT { get; set; }
        public string STATUSNFE { get; set; }
        public DateTime DTEMISSAO { get; set; }
        public DateTime DTENT { get; set; }
        public string CARTACORRECAONFE { get; set; }
        public string CNPJ_DESTINATARIO { get; set; }
        public string DESTINATARIO { get; set; }
        public string NUMTRANSACAO { get; set; }
        public string CFOP { get; set; }
        public string NUMPED { get; set; }
        public decimal VALORNF { get; set; }
        public string TIPONOTA { get; set; }
        public string NATUROPER { get; set; }
    }
}