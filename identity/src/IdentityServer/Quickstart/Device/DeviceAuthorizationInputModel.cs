// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using BeansIdentityService.Server.Quickstart.Consent;

namespace BeansIdentityService.Server.Quickstart.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public string UserCode { get; set; }
}
