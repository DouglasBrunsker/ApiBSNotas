using AutoMapper;
using Brunsker.Bsnotas.Application.Responses.NotasDia;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public class NotasDiaProfile : Profile
    {
        public NotasDiaProfile()
        {
            CreateMap<NotasDia, NotasDiaResponse>()
                .ForMember(nr => nr.Day, map => map.MapFrom(n => n.DIA))
                .ForMember(nr => nr.TotalNotes, map => map.MapFrom(n => n.TOT_NF))
                .ReverseMap();
        }
    }
}
