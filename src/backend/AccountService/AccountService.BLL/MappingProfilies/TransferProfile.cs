using AccountService.BLL.DTOs.Responses;
using AccountService.BLL.UseCases.Account.Commands.Transfer;
using AccountService.DAL.Entities;
using AutoMapper;

namespace AccountService.BLL.MappingProfilies;

public class TransferProfile : Profile
{
    public TransferProfile()
    {
        CreateMap<Transfer, TransferResponse>();
        CreateMap<TransferAccountCommand, Transfer>();
    }
}