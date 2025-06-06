﻿using AccountService.DAL.Contracts.Repositories;
using AutoMapper;
using MediatR;

namespace AccountService.BLL.UseCases.Account.Commands.Create;

public class CreateAccountCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper) : IRequestHandler<CreateAccountCommand>
{
    public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken = default)
    {
        var account = autoMapper.Map<DAL.Entities.Account>(request);
        await unitOfWork.AccountRepository.CreateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}