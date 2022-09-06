using AutoMapper;
using Brunsker.Bsnotas.Application.Requests.SearchNfse;
using Brunsker.Bsnotas.Domain.Models;

namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations.Profiles
{
    public class SearchNfseProfile : Profile
    {
        public SearchNfseProfile()
        {
            CreateMap<SearchNfse, SearchNfseRequest>()
                .ReverseMap();
        }
    }
}
