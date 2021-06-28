using System;
using System.Collections.Generic;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class ParametrosPesquisaNfEntrada
    {
        public int seqCliente { get; set; }
        public DateTime? DATAINI { get; set; }
        public DateTime? DATAFIM { get; set; }
        public string CHAVENFE { get; set; }
        public string NATUREZAOPER { get; set; }
        public long? NUMNOTA { get; set; }
        public string CNPJEMITENTE { get; set; }
        public string NOMEEMITENTE { get; set; }
        public string CNPJDEST { get; set; }
        public string NOMEDEST { get; set; }
        public DateTime[] DTENT { get; set; }
        public DateTime[] DTEMISSAO { get; set; }
        public string UF { get; set; }
        public int? CFOP { get; set; }
        public bool? EXIBIRNFELIVROFISCAL { get; set; }
        public bool? NFSEMENTRADAERP { get; set; }
        public bool? NOTASENTERP { get; set; }
        public bool? NOTASENTPREENT { get; set; }
        public bool? CARTACORRECAO { get; set; }
        public bool? DEVOLUCAO { get; set; }
        public bool? TRANSF { get; set; }
        public bool? EXCETOTRANSF { get; set; }
        public bool? STATUSNFEAUTORI { get; set; }
        public bool? STATUSNFECANC { get; set; }
        public bool? STATUSNFEDENEGADO { get; set; }
        public bool? STATUSMANIFNENHUMA { get; set; }
        public bool? STATUSMANIFCIENCIA { get; set; }
        public bool? STATUSMANIFCONFIRMADA { get; set; }
        public bool? STATUSMANIFDESCONHECIDA { get; set; }
        public bool? STATUSMANIFNAOREALIZADA { get; set; }
        public string[] EMPRESASCADASTRADAS { get; set; }
    }
}
