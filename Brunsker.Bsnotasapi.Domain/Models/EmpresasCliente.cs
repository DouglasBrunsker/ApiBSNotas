using Newtonsoft.Json;

namespace Brunsker.Bsnotas.Domain.Models
{
    public class EmpresasCliente
    {
        [JsonProperty("cnpj_Empresa")]
        public string Cnpj_Empresa { get; set; }

        [JsonProperty("nome_Empresa")]
        public string Nome_Empresa { get; set; }

        [JsonProperty("selected")]
        public bool Selected { get; set; }

    }
}
