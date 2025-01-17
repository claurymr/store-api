using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Store.Api.Contracts;
using Store.Api.Services;

namespace Store.Api.Endpoints.Auth;
public class CreateProductEndpoint(AuthSettings authSettings, IAuthService authService, IValidator<LoginRequest> validator)
    : Endpoint<LoginRequest, Results<Ok<TokenResponse>, BadRequest, ForbidHttpResult>>
{
    private readonly IAuthService _authService = authService;
    private readonly AuthSettings _authSettings = authSettings;
    private readonly IValidator<LoginRequest> _validator = validator;

    public override void Configure()
    {
        Verbs(Http.POST);
        Post("/api/login");

        Options(x =>
        {
            x.AllowAnonymous();
            x.WithDisplayName("Login to Inventory Management System");
            x.Produces<Ok<TokenResponse>>();
            x.Produces<BadRequest>();
            x.Produces<ForbidHttpResult>();
            x.WithOpenApi();
        });
    }

    public override async Task<Results<Ok<TokenResponse>, BadRequest, ForbidHttpResult>> ExecuteAsync(LoginRequest req, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid)
        {
            return (Results<Ok<TokenResponse>, BadRequest, ForbidHttpResult>)Results.BadRequest(validationResult);
        }

        if (req.UserName != _authSettings.UserName || req.Password != _authSettings.Password)
        {
            return (Results<Ok<TokenResponse>, BadRequest, ForbidHttpResult>)Results.Forbid();
        }

        var token = await _authService.GenerateToken(req.UserName);
        return (Results<Ok<TokenResponse>, BadRequest, ForbidHttpResult>)Results.Ok(token);
    }
}