using AutoMapper;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Entities;

namespace UserService.Application.MappingProfiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<CreateClientRequest, Client>();
        CreateMap<UpdateClientRequest, Client>();
        CreateMap<Client, ClientResponse>();
    }
}
