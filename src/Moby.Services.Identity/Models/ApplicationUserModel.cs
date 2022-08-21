using Microsoft.AspNetCore.Identity;

namespace Moby.Services.Identity.Models;

public class ApplicationUserModel : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}