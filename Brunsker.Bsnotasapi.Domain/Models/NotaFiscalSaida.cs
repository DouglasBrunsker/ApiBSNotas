using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class NotaFiscalSaida
    {
        public long NUMNOTA { get; set; }
        public double VLTOTAL { get; set; }
        public long CODCLI { get; set; }
        public string CLIENTE { get; set; }
        public long NUMTRANSVENDA { get; set; }
        public string CHAVENFE { get; set; }
        public string CGCENT { get; set; }
        public string IEENT { get; set; }
        public int SERIE { get; set; }
        public string CGCFILIAL { get; set; }
        public long CODFORNECFRETE { get; set; }
        public DateTime DTSAIDA { get; set; }
        public DateTime? DTCANCEL { get; set; }
    }
}