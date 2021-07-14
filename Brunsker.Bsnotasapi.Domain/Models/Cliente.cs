using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Cliente
    {
        public long CodCli { get; set; }
        public string Nome { get; set; }
        public string CgcEnt { get; set; }
        public string Inscricao { get; set; }
        public string Estado { get; set; }
        public string Bloqueio { get; set; }
        public string BloqueioSefaz { get; set; }
        public DateTime? DataValidacao { get; set; }
    }
}