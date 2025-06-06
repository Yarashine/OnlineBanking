﻿using System.Threading;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.UseCases.Authorization;

public interface IConfirmEmailUseCase
{
    Task ExecuteAsync(string userId, string token, CancellationToken cancellation = default);
}
