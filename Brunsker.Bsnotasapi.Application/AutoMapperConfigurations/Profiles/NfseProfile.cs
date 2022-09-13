using AutoMapper;
using Brunsker.Bsnotas.Application.Responses.Nfse;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public class NfseProfile : Profile
    {
        public NfseProfile()
        {
            CreateMap<Nfse, NfseResponse>()
                .ForMember(nr => nr.SeqArquivoXmlNfse, map => map.MapFrom(n => n.SEQ_ARQUIVOXML_NFSE))
                .ForMember(nr => nr.NickName, map => map.MapFrom(n => n.APELIDO))
                .ForMember(nr => nr.XmlFile, map => map.MapFrom(n => n.ARQUIVO_XML))
                .ForMember(nr => nr.SocialReasonProvider, map => map.MapFrom(n => n.PRESTADOR_RAZAO_SOCIAL))
                .ForMember(nr => nr.RPSNumber, map => map.MapFrom(n => n.NUMERO_RPS))
                .ForMember(nr => nr.ServiceValue, map => map.MapFrom(n => n.VALOR_SERVICO))
                .ForMember(nr => nr.SocialReasonTaker, map => map.MapFrom(n => n.TOMADOR_RAZAO_SOCIAL))
                .ForMember(nr => nr.SimpleNacionalOpt, map => map.MapFrom(n => n.OPT_SIMPLES_NACIONAL))
                .ForMember(nr => nr.IssRetained, map => map.MapFrom(n => n.ISS_RETIDO))
                .ForMember(nr => nr.Status, map => map.MapFrom(n => n.STATUS))
                .ForMember(nr => nr.EmissionDate, map => map.MapFrom(n => n.DT_EMISSAO))
                .ForMember(nr => nr.CnpjProvider, map => map.MapFrom(n => n.PRESTADOR_CNPJ))
                .ForMember(nr => nr.IssRetainedValue, map => map.MapFrom(n => n.VALOR_ISS_RETIDO))
                .ForMember(nr => nr.Observation, map => map.MapFrom(n => n.OBS))
                .ForMember(nr => nr.Descrimination, map => map.MapFrom(n => n.DESCRIMINACAO))
                .ReverseMap();
        }
    }
}
