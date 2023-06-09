// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BeansIdentityService.Server.Quickstart.Home;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IWebHostEnvironment _environment;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger _logger;

    public HomeController(
        IIdentityServerInteractionService interaction,
        IWebHostEnvironment environment,
        ILogger<HomeController> logger
    )
    {
        _interaction = interaction;
        _environment = environment;
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (_environment.IsDevelopment())
        {
            // only show in development
            return View();
        }

        _logger.LogInformation(
            "Homepage is disabled in production. Returning 404."
        );

        return NotFound();
    }

    /// <summary>
    ///     Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
    {
        ErrorViewModel vm = new ErrorViewModel();

        // retrieve error details from identityserver
        ErrorMessage message = await _interaction.GetErrorContextAsync(
            errorId
        );

        if (message != null)
        {
            vm.Error = message;

            if (!_environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }

        return View(
            "Error",
            vm
        );
    }
}
