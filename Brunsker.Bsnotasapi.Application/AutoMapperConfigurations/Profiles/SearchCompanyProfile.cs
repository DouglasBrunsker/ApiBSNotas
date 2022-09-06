using AutoMapper;
using Brunsker.Bsnotas.Application.Requests.SearchCompany;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public class SearchCompanyProfile : Profile
    {
        public SearchCompanyProfile()
        {
            CreateMap<SearchCompany, SearchCompanyRequest>()
                .ReverseMap();
        }
    }
}
