// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeansIdentityService.Server.Quickstart.Diagnostics;

[SecurityHeaders]
[Authorize]
public class DiagnosticsController : Controller
{
    public async Task<IActionResult> Index()
    {
        string[] localAddresses = new[]
        {
            "127.0.0.1",
            "::1",
            HttpContext.Connection.LocalIpAddress.ToString()
        };

        if (!localAddresses.Contains(
                HttpContext.Connection.RemoteIpAddress.ToString()
            ))
        {
            return NotFound();
        }

        DiagnosticsViewModel model = new DiagnosticsViewModel(
            await HttpContext.AuthenticateAsync()
        );

        return View(
            model
        );
    }
}
