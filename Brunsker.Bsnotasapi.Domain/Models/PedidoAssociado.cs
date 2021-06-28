using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class PedidoAssociado
    {
        public string CHAVE { get; set; }
        public DateTime DTEMISSAONF { get; set; }
        public DateTime DTEMISSAOPED { get; set; }
        public string CNPJ { get; set; }
        public long NUMPED { get; set; }
        public string PERCENTUAL { get; set; }
        public string DESCPLPAG { get; set; }
    }
}