using Application.Commons.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Commons.MappingProfiles;

public sealed class UrlResponseProfile : Profile
{
    public UrlResponseProfile()
    {
        CreateMap<Url, UrlResponse>()
            .ForMember(dest => dest.UrlId, opt => opt.MapFrom(src => src.UrlId))
            .ForMember(dest => dest.ShortUrl, opt => opt.MapFrom(src => src.ShortUrl))
            .ForMember(dest => dest.OriginalUrl, opt => opt.MapFrom(src => src.LongUrl));
    }
}