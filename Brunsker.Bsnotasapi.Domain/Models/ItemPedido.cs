namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class ItemPedido
    {
        public long CODPROD { get; set; }
        public string DESCRICAO { get; set; }
        public string EMBALAGEM { get; set; }
        public string UNIDADE { get; set; }
        public int QTPEDIDA { get; set; }
        public int QTPENTREGUE { get; set; }
        public double PCOMPRA { get; set; }
        public int NUMSEQ { get; set; }
    }
}