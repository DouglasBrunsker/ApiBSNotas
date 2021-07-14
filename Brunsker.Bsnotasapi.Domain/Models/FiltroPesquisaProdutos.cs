namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class FiltroPesquisaProdutos
    {
        public long SeqCliente { get; set; }
        public string NomeFornecedor { get; set; }
        public string NomeProduto { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
    }
}