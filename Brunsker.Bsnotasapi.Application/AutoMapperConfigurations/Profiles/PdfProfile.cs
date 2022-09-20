using AutoMapper;
using Brunsker.Bsnotas.Application.Requests.GeneratePdf;
using Brunsker.Bsnotas.Application.Responses.Pdf;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public sealed class PdfProfile : Profile
    {
        public PdfProfile()
        {
            CreateMap<GeneratePdfRequest, GeneratePdf>();

            CreateMap<Pdf, PdfResponse>()
                .ForMember(pr => pr.SeqArquivoXmlNfse, map => map.MapFrom(p => p.SEQ_ARQUIVOXML_NFSE))
                .ForMember(pr => pr.ArquivoPdf, map => map.MapFrom(p => p.ARQUIVOPDF))
                .ForMember(pr => pr.NomePdf, map => map.MapFrom(p => p.NOMEPDF));
        }
    }
}
