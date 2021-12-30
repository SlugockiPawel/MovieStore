
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

        public SeedService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }

        public async Task ManageDataAsync()
        {
            await UpdateDatabaseAsync();
            await SeedRoleAsync();
            await SeedUsersAsync();
            await SeedCollectionsAsync();
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

        private async Task SeedUsersAsync()
        {
            if (_userManager.Users.Any())
                return;


            var credentials = _appSettings.MovieStoreSettings.DefaultCredentials;
            var newUser = new IdentityUser()
            {
                Email = credentials.Email,
                UserName = credentials.Email,
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(newUser, credentials.Password);
            await _userManager.AddToRoleAsync(newUser, credentials.Role);
        }

        private async Task SeedCollectionsAsync()
        {
            if (_dbContext.Collections.Any())
                return;

            await _dbContext.Collections.AddAsync(new Collection()
            {
                Name = _appSettings.MovieStoreSettings.DefaultCollection.Name,
                Description = _appSettings.MovieStoreSettings.DefaultCollection.Description
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}