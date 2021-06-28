namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Usuario
    {
        public string NOME { get; set; }
        public long SEQ_CLIENTE { get; set; }
        public long SEQ_USUARIO { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }  
        public string AVATAR { get; set; }
    }
}