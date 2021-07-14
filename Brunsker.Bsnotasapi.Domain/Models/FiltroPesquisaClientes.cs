namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class FiltroPesquisaClientes
    {
        public long SeqCliente { get; set; }
        public string NomeCliente { get; set; }
        public string Cnpj { get; set; }
        public string StatusBloqueio { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
    }
}