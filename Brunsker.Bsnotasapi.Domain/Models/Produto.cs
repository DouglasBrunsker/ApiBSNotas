namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Produto
    {
        public long? CODPROD { get; set; } 
        public string DESCRICAO { get; set; }
        public long? CODAUXILIAR { get; set; }
        public string CODNCMEX { get; set; }
        public long? CODFISCAL { get; set; }
    }
}