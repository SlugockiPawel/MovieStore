using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieStore.Data;
using MovieStore.Models.Database;
using MovieStore.Models.Settings;

namespace MovieStore.Services
{
    public class SeedService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }


        private async Task UpdateDatabaseAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }

        private async Task SeedRoleAsync()
        {
            if (_dbContext.Roles.Any())
                return;


            var adminRole = _appSettings.MovieStoreSettings.DefaultCredentials.Role;


            await _roleManager.CreateAsync(new IdentityRole(adminRole));
        }

    }
}