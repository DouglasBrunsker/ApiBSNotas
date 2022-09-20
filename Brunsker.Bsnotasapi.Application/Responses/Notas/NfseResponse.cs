using System;

namespace Brunsker.Bsnotas.Application.Responses.Notas
{
    public class NfseResponse
    {
        public int SeqArquivoXmlNfse { get; set; }
        public string NickName { get; set; }
        public string XmlFile { get; set; }
        public string SocialReasonProvider { get; set; }
        public decimal RPSNumber { get; set; }
        public decimal ServiceValue { get; set; }
        public string SocialReasonTaker { get; set; }
        public string SimpleNacionalOpt { get; set; }
        public string IssRetained { get; set; }
        public string Status { get; set; }
        public DateTime EmissionDate { get; set; }
        public string CnpjProvider { get; set; }
        public decimal IssRetainedValue { get; set; }
        public string Observation { get; set; }
        public string Descrimination { get; set; }
    }
}
