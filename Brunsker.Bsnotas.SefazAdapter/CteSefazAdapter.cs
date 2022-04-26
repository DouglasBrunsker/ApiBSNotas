using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Interfaces;
using Brunsker.Bsnotasapi.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Brunsker.Bsnotas.SefazAdapter
{
    public class CteSefazAdapter : ICteSefazAdapter
    {
        private ILogger<CteSefazAdapter> _logger;
        private ICteRepository _rep;

        public CteSefazAdapter(ILogger<CteSefazAdapter> logger, ICteRepository rep)
        {
            _logger = logger;
            _rep = rep;
        }
        
        public async Task ManifestaCte(Manifestacao manifestacao, string webRootPath)
        {
            try
            {
                var tipoManifestacao = manifestacao.Codigo.Split('-');

                var ufAutorizacao = manifestacao.Chave.Substring(0, 2);

                var recepcao = await _rep.SelectRelacaoWebServices(manifestacao.SeqCliente, manifestacao.CnpjDestinatario, 1, int.Parse(ufAutorizacao));//1-produçao; 2-homologação

                if (recepcao != null)
                {
                    recepcao.CHAVE = manifestacao.Chave;
                    recepcao.CNPJ = manifestacao.CnpjDestinatario;
                    recepcao.TPEVENTO = tipoManifestacao[0];
                    recepcao.DESCEVENTO = tipoManifestacao[1];
                    recepcao.DHEVENTO = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "-03:00";
                    recepcao.JUSTIF = manifestacao.Justificativa;
                    recepcao.CERTIFICADO_DIGITAL = webRootPath + recepcao.CERTIFICADO_DIGITAL;

                    var resultado = RequestRecepcaoEvento(recepcao, manifestacao.SeqCliente);

                    if(resultado.cStat.Equals("135") || resultado.cStat.Equals("631"))
                    {
                        await _rep.ConfirmaManifestacaoCte(resultado.cStat, resultado.nProt, recepcao.CHAVE, manifestacao.SeqCliente, resultado.xMotivo, recepcao.TPEVENTO);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
        }

        private ResultadoCTe RequestRecepcaoEvento(RecepcaoEventoCte recepcao, int seqCliente)
        {
            string msg_padrao = "<eventoCTe  xmlns=\"http://www.portalfiscal.inf.br/cte\" versao=\"" + recepcao.VERSAO_EVENTO + "\">"
                    + "<infEvento Id=\"ID" + recepcao.TPEVENTO + recepcao.CHAVE + "01\">"
                    + "<cOrgao>"+ recepcao.CORGAO + "</cOrgao>"
                    + "<tpAmb>" + recepcao.TPAMB + "</tpAmb>"
                    + "<CNPJ>" + recepcao.CNPJ + "</CNPJ>"
                    + "<chCTe>" + recepcao.CHAVE + "</chCTe>"
                    + "<dhEvento>" + recepcao.DHEVENTO + "</dhEvento>"
                    + "<tpEvento>" + recepcao.TPEVENTO + "</tpEvento>"
                    + "<nSeqEvento>" + recepcao.NSEQEVENTO + "</nSeqEvento>"
                    + "<detEvento versaoEvento=\"" + recepcao.VERSAO_DET_EVENTO + "\">"
                    + "<evPrestDesacordo>"
                            + "<descEvento>" + recepcao.DESCEVENTO + "</descEvento>"
                            + "<indDesacordoOper>1</indDesacordoOper>"
                            + "<xObs>" + recepcao.JUSTIF + "</xObs>"
                        + "</evPrestDesacordo>"
                    + "</detEvento></infEvento></eventoCTe>";

            XmlDocument xmlAss = new XmlDocument();

            xmlAss.LoadXml(AssinaXML(msg_padrao, recepcao.CHAVE, recepcao.TPEVENTO, recepcao.CERTIFICADO_DIGITAL, recepcao.SENHA).OuterXml);

            string msg_soap = "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\"><soap12:Header>"+
                    "<cteCabecMsg xmlns=\"http://www.portalfiscal.inf.br/cte/wsdl/CteRecepcaoEvento\">" +
                    "<cUF>" + recepcao.CORGAO + "</cUF>" +
                    "<versaoDados>" + "3.00" + "</versaoDados>" +
                    "</cteCabecMsg>"+
                    "</soap12:Header>" +
                    "<soap12:Body>" +
                    "<cteDadosMsg xmlns=\"http://www.portalfiscal.inf.br/cte/wsdl/CteRecepcaoEvento\">" 
                    + xmlAss.InnerXml + "</cteDadosMsg>" +
                    "</soap12:Body></soap12:Envelope>";

            XmlDocument Resposta_WS = new XmlDocument();

            string result_request = "";

            ResultadoCTe retorno = new ResultadoCTe();

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

                retorno.cStat = Resposta_WS?.GetElementsByTagName("cStat")?.Item(0)?.FirstChild?.Value;

                retorno.xMotivo = Resposta_WS?.GetElementsByTagName("xMotivo")?.Item(0)?.FirstChild?.Value;

                retorno.dhRegEvento = Resposta_WS?.GetElementsByTagName("dhRegEvento")?.Item(0)?.FirstChild?.Value;

                retorno.nProt = Resposta_WS?.GetElementsByTagName("nProt")?.Item(0)?.FirstChild?.Value;

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro nos dados de retorno da SEFAZ: " + ex.Message);
            }
            if (retorno.cStat != "135")
            {
                LogErroManifestacaoCte(retorno.cStat, retorno.xMotivo, recepcao.CHAVE, seqCliente);

                _logger.LogError("cSat: " + retorno.cStat + " | " + retorno.xMotivo);
            }
            return (retorno);
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

                xml.GetElementsByTagName("eventoCTe").Item(0).AppendChild(xml.ImportNode(xmlDigitalSignature, true));

                return xml;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
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
        private async void LogErroManifestacaoCte(string cStat, string xMotivo, string chCte, int seqCliente)
        {
            await _rep.LogErroManifestacaoCte(cStat, xMotivo, chCte, seqCliente);
        }
    }
}