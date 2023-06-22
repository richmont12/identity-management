// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using BeansIdentityService.Server.Quickstart.Consent;

namespace BeansIdentityService.Server.Quickstart.Device;

public class DeviceAuthorizationViewModel : ConsentViewModel
{
    public string UserCode { get; set; }
    public bool ConfirmUserCode { get; set; }
}
