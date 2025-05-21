using AutoMapper;
using UserService.Application.DTOs.Requests;
using UserService.Domain.Entities;

namespace UserService.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<SignUpRequest, User>();
    }
}
