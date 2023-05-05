using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
    public ActionResult<MightyData> GetMightyData()
    {
        return Ok(new MightyData
        {
            Id = Guid.NewGuid(),
            Api1Data = "Api 1 Data that is mighty"
        });
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