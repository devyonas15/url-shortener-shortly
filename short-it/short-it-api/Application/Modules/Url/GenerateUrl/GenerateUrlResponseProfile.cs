using AutoMapper;
using UrlEntity = Domain.Entities.Url;

namespace Application.Modules.Url.GenerateUrl;

public sealed class GenerateUrlResponseProfile : Profile
{
    public GenerateUrlResponseProfile()
    {
        CreateMap<UrlEntity, GenerateUrlResponse>()
            .ForMember(dest => dest.Success, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UrlId))
            .ForMember(dest => dest.ShortUrl, opt => opt.MapFrom(src => src.ShortUrl));
    }
}