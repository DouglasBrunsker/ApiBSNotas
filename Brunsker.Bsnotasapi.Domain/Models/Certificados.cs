using System;

namespace Brunsker.Bsnotasapi.Domain.Models
{
    public class Certificado
    {
        public long SeqCliente { get; set; }
        public DateTime Validade { get; set; }
        public string NomeCertificado { get; set; }
    }
}