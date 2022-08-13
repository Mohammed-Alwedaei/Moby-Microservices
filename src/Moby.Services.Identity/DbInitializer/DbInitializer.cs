using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Moby.Services.Identity.DbContexts;
using Moby.Services.Identity.Models;

namespace Moby.Services.Identity.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            if (await _roleManager.FindByNameAsync(SD.Admin) is null)
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Admin));
                await _roleManager.CreateAsync(new IdentityRole(SD.Customer));
            }
            else { return; }

            await CreateUser(new ApplicationUserModel
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                PhoneNumber = "39308900",
                FirstName = "Mohammed",
                LastName = "Ali"
            }, SD.Admin);

            await CreateUser(new ApplicationUserModel
            {
                UserName = "customer@customer.com",
                Email = "customer@customer.com",
                EmailConfirmed = true,
                PhoneNumber = "39973804",
                FirstName = "Jassim",
                LastName = "Khalid"
            }, SD.Customer);
        }

        public async Task CreateUser(ApplicationUserModel user, string role)
        {
            await _userManager.CreateAsync(user, $"TestUser@123");
            await _userManager.AddToRoleAsync(user, role);

            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Role, role),
            });
        }
    }
}
