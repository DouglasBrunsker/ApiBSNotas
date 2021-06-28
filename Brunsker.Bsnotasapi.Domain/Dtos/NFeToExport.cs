using System;

namespace Brunsker.Bsnotasapi.Domain.Dtos
{
    public class NFeToExport
    {
        public string CHAVE { get; set; }
        public string CNPJ_EMITENTE { get; set; }
        public string EMITENTE { get; set; }
        public string STATUSMANIF { get; set; }
        public long NUMNOTA { get; set; }
        public string STATUSPREENT { get; set; }
        public string STATUSNFE { get; set; }
        public DateTime DTREC { get; set; }
        public DateTime DTENT { get; set; }
        public string CARTACORRECAONFE { get; set; }
        public string CNPJ_DESTINATARIO { get; set; }
        public string DESTINATARIO { get; set; }
        public string NUMTRANSACAO { get; set; }
        public string CFOP { get; set; }
        public string NUMPED { get; set; }
        public decimal VALORNF { get; set; }
        public string TIPONOTA { get; set; }
    }
}