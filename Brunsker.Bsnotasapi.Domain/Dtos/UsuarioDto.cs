using Newtonsoft.Json;

namespace Brunsker.Bsnotasapi.Domain.Dtos
{
    public class UsuarioDto
    {
        public string NOME { get; set; }

        [JsonProperty("seq_cliente")]
        public long SEQ_CLIENTE { get; set; }
        public string LOGIN { get; set; }
        public string TOKEN { get; set; }
        public string AVATAR { get; set; }
    }
}