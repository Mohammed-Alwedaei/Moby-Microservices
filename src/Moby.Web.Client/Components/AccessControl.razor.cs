using Microsoft.AspNetCore.Components.Web;

namespace Moby.Web.Client.Components;

public partial class AccessControl
{
    [Inject]
    NavigationManager Navigation { get; set; }

    [Inject]
    SignOutSessionStateManager SignOutManager { get; set; }

    /// <summary>
    /// Begin user sign out process
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private async Task BeginSignOut(MouseEventArgs args)
    {
        await SignOutManager.SetSignOutState();
        Navigation.NavigateTo("authentication/logout");
    }
}
