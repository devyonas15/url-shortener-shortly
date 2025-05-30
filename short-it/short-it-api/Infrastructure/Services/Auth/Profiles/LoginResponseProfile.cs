using Application.Modules.Auth.Login;
using AutoMapper;
using Persistence.Models;

namespace Infrastructure.Services.Auth.Profiles;

public sealed class LoginResponseProfile : Profile
{
    public LoginResponseProfile()
    {
        CreateMap<ApplicationUser, LoginResponse>()
            .ForMember(dest => dest.LoginId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}