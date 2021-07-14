namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Produto
    {
        public long CodProd { get; set; }
        public string Descricao { get; set; }
        public long? CodAuxiliar { get; set; }
        public string CodNcmEx { get; set; }
        public long? CodFiscal { get; set; }
    }
}