using AutoMapper;
using Brunsker.Bsnotas.Application.Responses.Totalizador;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public class TotalizadorProfile : Profile
    {
        public TotalizadorProfile()
        {
            CreateMap<Totalizador, TotalizadorResponse>()
                .ForMember(tr => tr.Quantity, map => map.MapFrom(t => t.QTNF))
                .ForMember(tr => tr.TotalValue, map => map.MapFrom(t => t.VLTOTSERV))
                .ForMember(tr => tr.Canceled, map => map.MapFrom(t => t.QTCANCEL))
                .ForMember(tr => tr.IssValueRetained, map => map.MapFrom(t => t.VLISSRETIDO))
                .ReverseMap();
        }
    }
}
