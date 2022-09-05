namespace Moby.Web.Client.Pages.Authentication;

public partial class Authentication
{
    [Inject]
    NavigationManager Navigation { get; set; }

    [Inject]
    IConfiguration Configuration { get; set; }

    [Parameter] public string Action { get; set; }
}
