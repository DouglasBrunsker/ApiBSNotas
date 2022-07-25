using System.ComponentModel.DataAnnotations;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class ValidarPreEntrada
    {
        public int SEQ_CLIENTE { get; set; }

        [Required]
        public int CODFILIAL { get; set; }
        public string CHAVE { get; set; }
        public long? NUMPED { get; set; }
        public string COPROD { get; set; }
    }
}