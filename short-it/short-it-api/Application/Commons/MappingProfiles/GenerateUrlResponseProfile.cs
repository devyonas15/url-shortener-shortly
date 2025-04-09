using Application.Commons.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.MappingProfiles;

public sealed class GenerateUrlResponseProfile : Profile
{
    public GenerateUrlResponseProfile()
    {
        CreateMap<Url, GenerateUrlResponse>()
            .ForMember(dest => dest.Success, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UrlId))
            .ForMember(dest => dest.ShortUrl, opt => opt.MapFrom(src => src.ShortUrl));
    }
}