using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Brunsker.Bsnotas.SefazAdapter
{
    public class SefazApiAdapter : ISefazApiAdapter
    {
        private ILogger<SefazApiAdapter> _logger;
        private INFEntradaRepository _rep;
        public SefazApiAdapter(ILogger<SefazApiAdapter> logger, INFEntradaRepository rep)
        {
            _logger = logger;
            _rep = rep;
        }

        public async Task ManifestaNotas(Manifestacao manifestacao, string webRootPath)
        {
            try
            {
                var tipoManifestacao = manifestacao.Codigo.Split('-');

                RecepcaoEvento recepcao = await _rep.SelectRelacaoWebServices(manifestacao.SeqCliente, manifestacao.CnpjDestinatario);

                if (recepcao != null)
                {
                    recepcao.CHAVE = manifestacao.Chave;
                    recepcao.CNPJ = manifestacao.CnpjDestinatario;
                    recepcao.TPEVENTO = tipoManifestacao[0];
                    recepcao.DESCEVENTO = tipoManifestacao[1];
                    recepcao.DHEVENTO = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "-03:00";
                    recepcao.JUSTIF = manifestacao.Justificativa.Trim();
                    recepcao.CERTIFICADO_DIGITAL = webRootPath + recepcao.CERTIFICADO_DIGITAL;

                    var resultado = RequestRecepcaoEvento(recepcao);

                    if (resultado.cStat.Equals("573") || resultado.cStat.Equals("135") || resultado.cStat.Equals("128"))
                    {
                        string codigo = recepcao.TPEVENTO.Replace("210200", "1").Replace("210210", "4").Replace("210220", "2").Replace("210240", "3");

                        await _rep.ConfirmaManifestacao(recepcao.CHAVE, codigo);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
        }

        private Resultado RequestRecepcaoEvento(RecepcaoEvento recepcao)
        {
            string msg_padrao = "<envEvento xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"" + recepcao.VERSAO + "\"><idLote>1</idLote><evento versao=\"" + recepcao.VERSAO + "\"><infEvento Id=\"ID" + recepcao.TPEVENTO + recepcao.CHAVE + "01\"><cOrgao>" + recepcao.CORGAO + "</cOrgao><tpAmb>" + recepcao.TPAMB + "</tpAmb><CNPJ>" + recepcao.CNPJ + "</CNPJ><chNFe>" + recepcao.CHAVE + "</chNFe><dhEvento>" + recepcao.DHEVENTO + "</dhEvento><tpEvento>" + recepcao.TPEVENTO + "</tpEvento><nSeqEvento>" + recepcao.NSEQEVENTO + "</nSeqEvento><verEvento>" + recepcao.VEREVENTO + "</verEvento><detEvento versao=\"" + recepcao.VEREVENTO + "\"><descEvento>" + recepcao.DESCEVENTO + "</descEvento>" + ((!String.IsNullOrWhiteSpace(recepcao.JUSTIF)) ? "<xJust>" + recepcao.JUSTIF + "</xJust>" : "") + "</detEvento></infEvento></evento></envEvento>";

            XmlDocument xmlAss = new XmlDocument();

            xmlAss.LoadXml(AssinaXML(msg_padrao, recepcao.CHAVE, recepcao.TPEVENTO, recepcao.CERTIFICADO_DIGITAL, recepcao.SENHA).OuterXml);

            string msg_soap = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:nfer=\"http://www.portalfiscal.inf.br/nfe/wsdl/NFeRecepcaoEvento4\"><soapenv:Header/><soapenv:Body><nfer:nfeDadosMsg>" + xmlAss.InnerXml + "</nfer:nfeDadosMsg></soapenv:Body></soapenv:Envelope>";

            XmlDocument Resposta_WS = new XmlDocument();

            string result_request = "";

            Resultado retorno = new Resultado();

            try
            {
                result_request = SoapWebRequest(recepcao.URL, msg_soap, recepcao.CERTIFICADO_DIGITAL, recepcao.SENHA);
            }
            catch (WebException ex)
            {
                _logger.LogError("Erro na conexão com o webservice: " + ex.Message);
            }
            try
            {
                Resposta_WS.LoadXml(result_request);

                retorno.cStat = Resposta_WS?.GetElementsByTagName("cStat")?.Item(1)?.FirstChild?.Value;

                retorno.xMotivo = Resposta_WS?.GetElementsByTagName("xMotivo")?.Item(1)?.FirstChild?.Value;

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro nos dados de retorno da SEFAZ: " + ex.Message);
            }
            if (retorno.cStat != "135")
            {
                _logger.LogError("cSat: " + retorno.cStat + " | " + retorno.xMotivo);
            }
            return (retorno);
        }
        private string SoapWebRequest(string url, string msg, string path, string senha)
        {
            try
            {
                HttpClient client = CriaWebRequest(@url, @path, senha);

                XmlDocument soapEnvelopeXml = new XmlDocument();

                soapEnvelopeXml.LoadXml(msg);

                var content = new StringContent(soapEnvelopeXml.InnerXml.ToString(), Encoding.UTF8, "text/xml");

                var response = client.PostAsync(url, content).Result;

                var result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }

        }
        private HttpClient CriaWebRequest(string url, string path, string senha)
        {
            try
            {
                var handler = new HttpClientHandler();

                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                handler.ClientCertificateOptions = ClientCertificateOption.Manual;

                handler.SslProtocols = SslProtocols.Tls12;

                X509Certificate cert = new X509Certificate2(path, senha);

                handler.ClientCertificates.Add(cert);

                return new HttpClient(handler);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
        }
        private XmlDocument AssinaXML(string xmlToSigne, string chave, string tipoEvento, string certificado, string senha)
        {
            try
            {
                XmlDocument xml = new XmlDocument();

                xml.LoadXml(@xmlToSigne);

                SignedXml signedXml = new SignedXml(xml);

                X509Certificate2 Cert = new X509Certificate2(@certificado, senha);

                signedXml.SigningKey = Cert.PrivateKey;

                Reference reference = new Reference();

                reference.Uri = "#ID" + tipoEvento + chave + "01";

                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();

                reference.AddTransform(env);

                XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();

                reference.AddTransform(c14);

                reference.DigestMethod = SignedXml.XmlDsigSHA1Url;

                signedXml.AddReference(reference);

                KeyInfo keyInfo = new KeyInfo();

                keyInfo.AddClause(new KeyInfoX509Data(Cert));

                signedXml.KeyInfo = keyInfo;

                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

                signedXml.ComputeSignature();

                XmlElement xmlDigitalSignature = signedXml.GetXml();

                xml.GetElementsByTagName("evento").Item(0).AppendChild(xml.ImportNode(xmlDigitalSignature, true));

                return xml;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
        }
    }
}
