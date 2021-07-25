namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Usuario
    {
        public long SEQ_USUARIOS { get; set; }
        public string NOME { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public long SEQ_CLIENTE { get; set; }
        public string AVATAR { get; set; }
    }
}