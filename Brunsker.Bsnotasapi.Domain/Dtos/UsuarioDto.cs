namespace Brunsker.Bsnotasapi.Domain.Dtos
{
    public class UsuarioDto
    {
        public string NOME { get; set; }
        public long SEQ_CLIENTE { get; set; }
        public long SEQ_USUARIOS { get; set; }
        public string LOGIN { get; set; }
        public string TOKEN { get; set; }
        public string AVATAR { get; set; }
        public string NFESERVICO_ATIVO { get; set; }
    }
}