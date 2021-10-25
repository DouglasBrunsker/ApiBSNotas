using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class ParametrosPesquisaCte
    {
        public int SEQ_CLIENTE { get; set; }
        public DateTime? DATAINI { get; set; }
        public DateTime? DATAFIM { get; set; }
        public DateTime[] DTENT { get; set; }
        public DateTime[] DTEMISSAO { get; set; }
        public string UF { get; set; }
        public long? NUMNOTA { get; set; }
        public bool? EXIBIRCTEMANIFACORDO { get; set; }
        public bool? EXIBIRCTECARTACORRECAO { get; set; }
        public bool? EXIBIRCTECLITOMADOR { get; set; }
        public bool? STATUSCTEAUTORI { get; set; }
        public bool? STATUSCTECANC { get; set; }
        public bool? STATUSCTEDENEGADO { get; set; }
        public bool? STATUSMANIFDESACORDO { get; set; }
        public string NATUREZAOPER { get; set; }
        public string CHAVECTE { get; set; }
        public string CNPJEMITENTE { get; set; }
        public string NOMEEMITENTE { get; set; }
        public string CNPJDEST { get; set; }
        public string NOMEDEST { get; set; }
        public string[] EMPRESASCADASTRADAS { get; set; }
    }
}