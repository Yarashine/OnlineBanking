using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Application.Contracts.Services;
using UserService.Application.Contracts.UseCases.Authorization;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Configs;
using UserService.Domain.Constants;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.Authorization;

public class SignUpUseCase(
    IMapper autoMapper,
    UserManager<User> userManager,
    ITokenService tokenService,
    IOptions<KafkaOptions> options,
    IProducer<Null, string> kafkaProducer,
    ILogger<SignUpUseCase> logger) : ISignUpUseCase
{
    private readonly KafkaOptions kafkaEmailConfirmation = options.Value;

    public async Task<TokensResponse> ExecuteAsync(SignUpRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting user registration for email: {Email}", request.Email);

        var user = autoMapper.Map<User>(request);
        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Errors.Any())
        {
            var error = result.Errors.First();
            var (code, message) = MapIdentityError(error);

            logger.LogWarning("Failed to register user {Email}. ErrorCode: {Code}, Message: {Message}", request.Email, code, message);

            if (code == "USERNAME_EXISTS" || code == "EMAIL_EXISTS")
            {
                throw new AlreadyExistsException(message);
            }
            else
            {
                throw new BadRequestException(message);
            }
        }

        logger.LogInformation("User created successfully: {Email}", request.Email);

        await userManager.AddToRoleAsync(user, Role.Client.ToString());

        var confirmMessage = JsonSerializer.Serialize(new
        {
            Email = request.Email,
        });

        logger.LogInformation("Sending email confirmation for user: {Email}", request.Email);

        await kafkaProducer.ProduceAsync("email-confirmation", new Message<Null, string> { Value = confirmMessage }, cancellationToken);

        logger.LogInformation("Email confirmation sent to Kafka for user: {Email}", request.Email);

        var tokens = await tokenService.GenerateTokensAsync(user.Id.ToString(), request.DeviceId, Role.Client.ToString(), cancellationToken);

        logger.LogInformation("Tokens generated for user: {UserId}", user.Id);

        return new TokensResponse
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
        };
    }


    private static (string Code, string Message) MapIdentityError(IdentityError error)
    {
        return error.Code switch
        {
            "DuplicateUserName" => ("USERNAME_EXISTS", "Username already taken"),
            "DuplicateEmail" => ("EMAIL_EXISTS", "Email already registered"),
            _ => ("UNKNOWN_ERROR", error.Description)
        };
    }
}
