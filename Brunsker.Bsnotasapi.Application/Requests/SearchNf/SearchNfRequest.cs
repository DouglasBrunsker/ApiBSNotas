using System;

namespace Brunsker.Bsnotas.Application.Requests.SearchNf
{
    public class SearchNfRequest
    {
        public int SeqCliente { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string? PrestadorCnpj { get; set; }
        public string? TomadorCnpj { get; set; }
        public int? NumeroRps { get; set; }
        public string? PrestadorRazaoSocial { get; set; }
        public string? TomadorRazaoSocial { get; set; }
        public int? StatusNormal { get; set; } = 0;
        public int? StatusCancelada { get; set; } = 0;
        public int? StatusExtraviada { get; set; } = 0;
        public int? StatusLote { get; set; } = 0;
        public string? EmpresasCadastradas { get; set; }
    }
}
