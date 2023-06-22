using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdentityModel;

namespace IdentityServer;

public class TokenExchangeGrantValidator : IExtensionGrantValidator
{
    private static readonly Dictionary<string, object> s_customResponse = new()
    {
        {
            OidcConstants.TokenResponse.IssuedTokenType,
            OidcConstants.TokenTypeIdentifiers.AccessToken
        }
    };
    private readonly ITokenValidator _validator;

    public TokenExchangeGrantValidator(ITokenValidator validator)
    {
        _validator = validator;
    }

     public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        TokenValidationResult validationResult = await ValidateTokenAsync(
            context
        );

        if (validationResult is null || validationResult.IsError)
        {
            return;
        }

        SetActorClaim(
            context,
            validationResult.Claims.First(
                    c => c.Type == JwtClaimTypes.ClientId
                )
                .Value,
            validationResult.Claims.First(
                    c => c.Type == JwtClaimTypes.Subject
                )
                .Value
        );
    }

    private async Task<TokenValidationResult> ValidateTokenAsync(
        ExtensionGrantValidationContext context
    )
    {
        // The default response is an error response
        context.Result = new GrantValidationResult(
            TokenRequestErrors.InvalidRequest
        );

        string subjectToken = context.Request.Raw.Get(
            OidcConstants.TokenRequest.SubjectToken
        );

        string subjectTokenType = context.Request.Raw.Get(
            OidcConstants.TokenRequest.SubjectTokenType
        );

        // Validate the token subject type
        if (string.IsNullOrWhiteSpace(
                subjectToken
            )
            || string.Equals(
                subjectTokenType,
                OidcConstants.TokenTypeIdentifiers.AccessToken
            ) is false)
        {
            return null;
        }

        // Validate the token content
        return await _validator.ValidateAccessTokenAsync(
            subjectToken
        );
    }

    private void SetActorClaim(
        ExtensionGrantValidationContext context,
        string clientId,
        string sub
    )
    {
        // Use the original client id for  the claim 'client_id'
        context.Request.ClientId = clientId;

        var actor = new
        {
            client_id = context.Request.Client.ClientId
        };

        Claim actClaim = new(
            JwtClaimTypes.Actor,
            JsonSerializer.Serialize(
                actor
            ),
            IdentityServerConstants.ClaimValueTypes.Json
        );

        context.Result = new GrantValidationResult(
            sub,
            GrantType,
            new[]
            {
                actClaim
            },
            customResponse: s_customResponse
        );
    }
    public string GrantType => OidcConstants.GrantTypes.TokenExchange;
}