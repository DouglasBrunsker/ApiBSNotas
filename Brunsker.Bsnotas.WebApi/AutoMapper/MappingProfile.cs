using AutoMapper;
using Brunsker.Bsnotasapi.Domain.Dtos;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotas.WebApi.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NF, NFeToExport>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
        }
    }
}