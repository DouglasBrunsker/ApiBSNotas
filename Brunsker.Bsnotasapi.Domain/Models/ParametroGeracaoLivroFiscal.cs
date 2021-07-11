using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class ParametroGeracaoLivroFiscal
    {
        public string Chave { get; set; }
        public long CodigoFornecedor { get; set; }
        public long NumeroNota { get; set; }
        public DateTime DataEmissao { get; set; }
        public long SeqCliente { get; set; }
    }
}