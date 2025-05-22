using AccountService.BLL.DTOs.Responses;
using AccountService.BLL.UseCases.Account.Commands.Create;
using AccountService.BLL.UseCases.Account.Commands.Update;
using AccountService.DAL.Entities;
using AutoMapper;

namespace AccountService.BLL.MappingProfilies;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<CreateAccountCommand, Account>();
        CreateMap<UpdateAccountCommand, Account>();
        CreateMap<Account, AccountResponse>();
    }
}