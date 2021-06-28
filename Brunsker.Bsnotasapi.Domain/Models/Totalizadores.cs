using System.Collections.Generic;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class Totalizadores
    {
        public long QTNF { get; set; }
        public decimal VLTOTAL { get; set; }
        public long QTNFCANCELADAS { get; set; }
        public long QTNFMANIF { get; set; }
        public long QTPREENT_REALIZADAS { get; set; }
        public long QTNF_SEM_ENTRADAS { get; set; }
        public long QTNF_SEM_ENTRADAS_COMDIV { get; set; }
        public long QTNF_ERP { get; set; }
        public long QTNF_ENTRADAS_ANALISE { get; set; }
        public IEnumerable<TotalizadorNotasPorDia> TotalNotasPorDia { get; set; }
    }
}
