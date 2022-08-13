using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Moby.Services.Identity.Models;

namespace Moby.Services.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUserModel> _userClaimsPrincipalFactory;

        public ProfileService(UserManager<ApplicationUserModel> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IUserClaimsPrincipalFactory<ApplicationUserModel> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sub = context.Subject.GetSubjectId();

            ApplicationUserModel user = await _userManager.FindByIdAsync(sub);

            ClaimsPrincipal claimsPrincipal = await _userClaimsPrincipalFactory.CreateAsync(user);

            List<Claim> claims = new();

            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

            if (_userManager.SupportsUserRole)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, role));

                    if (_roleManager.SupportsRoleClaims)
                    {
                        IdentityRole identityRole = await _roleManager.FindByNameAsync(role);

                        if (identityRole is not null)
                            claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                    }
                }
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUserModel user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
