using AutoMapper;
using Brunsker.Bsnotas.Application.Responses.Company;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyResponse>()
                .ForMember(cr => cr.CompanyCnpj, map => map.MapFrom(c => c.CNPJ_EMPRESA))
                .ForMember(cr => cr.CompanyName, map => map.MapFrom(c => c.NOME_EMPRESA))
                .ReverseMap();
        }
    }
}
