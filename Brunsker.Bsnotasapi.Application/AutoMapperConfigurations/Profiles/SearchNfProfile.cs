using AutoMapper;
using Brunsker.Bsnotas.Application.Requests.SearchNf;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public class SearchNfProfile : Profile
    {
        public SearchNfProfile()
        {
            CreateMap<SearchNf, SearchNfRequest>()
                .ReverseMap();
        }
    }
}
