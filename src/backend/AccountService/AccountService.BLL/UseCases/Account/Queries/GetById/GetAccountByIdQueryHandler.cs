﻿using AccountService.BLL.DTOs.Responses;
using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Exceptions;
using AutoMapper;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Queries.GetById;

public class GetAccountByIdQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper) : IRequestHandler<GetAccountByIdQuery, AccountResponse>
{
    public async Task<AccountResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken = default)
    {
        var accountForCheck = await unitOfWork.AccountRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Account");
        var account = await unitOfWork.AccountRepository.GetByIdAsync(request.Id, cancellationToken);

        return autoMapper.Map<AccountResponse>(account);
    }
}