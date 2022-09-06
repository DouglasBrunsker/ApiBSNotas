using System;

namespace Brunsker.Bsnotas.Application.Requests.SearchNfse
{
    public class SearchNfseRequest
    {
        public int SeqCliente { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
    }
}
