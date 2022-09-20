using AutoMapper;
using Brunsker.Bsnotas.Application.Requests.Searchs;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public sealed class SearchsProfile : Profile
    {
        public SearchsProfile()
        {
            CreateMap<SearchCompany, SearchCompanyRequest>()
                .ReverseMap();

            CreateMap<SearchNfse, SearchNfseRequest>()
                .ReverseMap();

            CreateMap<SearchNf, SearchNfRequest>()
                .ReverseMap();
        }
    }
}
