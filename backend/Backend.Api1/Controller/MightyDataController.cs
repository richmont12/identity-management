using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api1.Controller;

public class MightyDataController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MightyDataController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    [HttpGet("api/backend/1/mighty")]
    public async Task<ActionResult<MightyData>> GetMightyData()
    {
        var clientWithExchangedToken = await GetClientWithExchangedTokenAsync();
        return Ok(new MightyData
        {
            Id = Guid.NewGuid(),
            Api1Data = "Api 1 Data that is mighty"
        });
    }
    
    [HttpGet("api/backend/1/mighty2")]
    public async Task<ActionResult<MightyData>> GetMightyData2()
    {
        var clientWithExchangedToken = await GetClientWithExchangedTokenAsync();
        var mightyData =
            await clientWithExchangedToken
                .GetFromJsonAsync<MightyData>("https://localhost:6002/api/backend/2/mighty");
        return Ok(mightyData);
    }
    

    private async Task<HttpClient> GetClientWithExchangedTokenAsync()
    {
        const string authorityUrl = "https://localhost:5001";
        const string clientId = "machineclientdelegation";
        const string clientSecret = "secret";
        List<string> scopes = new List<string>()
        {
            "dataapi2"
        };

        var bearerJwtTokenValue = Request.Headers.Authorization
            .Select(x => x).Single().Replace("Bearer ", string.Empty);

        HttpClient client = _httpClientFactory.CreateClient();

        DiscoveryDocumentResponse discoveryDocumentResponse =
            await client.GetDiscoveryDocumentAsync(
                authorityUrl
            );

        if (discoveryDocumentResponse.IsError)
        {
            throw new Exception(
                $"Failed to get discovery document from {authorityUrl}: " +
                $"{discoveryDocumentResponse.Error}");
        }

        TokenResponse tokenResponseClientCredentials =
            await client.RequestTokenExchangeTokenAsync(
                new TokenExchangeTokenRequest
                {
                    Address = discoveryDocumentResponse.TokenEndpoint,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Scope = scopes?.Aggregate(
                        (x, y) => $"{x} {y}"
                    ),
                    SubjectToken = bearerJwtTokenValue,
                    SubjectTokenType =
                        OidcConstants.TokenTypeIdentifiers.AccessToken
                }
            );

        if (tokenResponseClientCredentials.IsError)
        {
            throw new Exception(
                $"Failed to get exchange token from {discoveryDocumentResponse.TokenEndpoint}: " +
                $"{tokenResponseClientCredentials.Raw}");
        }

        var clientWithExchangedToken = _httpClientFactory.CreateClient();
        clientWithExchangedToken.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenResponseClientCredentials.AccessToken);
        
        return clientWithExchangedToken;
    }

    [AllowAnonymous]
    [HttpGet("api/backend/1/mighty-anonymous")]
    public async Task<ActionResult<MightyData>> GetMightyDataAnonymous()
    {
        ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => true;

        var client = _httpClientFactory.CreateClient();
        var data = await client.GetAsync("https://localhost:5001/.well-known/openid-configuration");


        return Ok(new MightyData
        {
            Id = Guid.NewGuid(),
            Api1Data = "Api 1 Data that is mighty"
        });
    }
}

public class MightyData
{
    public Guid Id { get; set; }
    public string Api1Data { get; set; } = null!;
}