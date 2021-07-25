namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class FiltroPesquisaClientes
    {
        public long SeqCliente { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Bloqueio { get; set; }
    }
}