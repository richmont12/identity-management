using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api1.Controller;

public class MightyDataController : ControllerBase
{
    [HttpGet("api/backend/1/mighty")]
    public ActionResult<MightyData> GetMightyData()
    {
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