using EntityFrameworkCore.UnitOfWork.Interfaces;
using KeyStone.Concerns.Domain;
using KeyStone.Concerns.Identity;
using KeyStone.Identity.Wrappers.Managers;
using Microsoft.EntityFrameworkCore;

namespace KeyStone.Identity.Seed
{
    public interface IIdentityDatabaseSeeder
    {
        Task Seed();
    }
    public class IdentityDatabaseSeeder : IIdentityDatabaseSeeder
    {
        private readonly AppUserManager _userManager;
        private readonly AppRoleManager _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public IdentityDatabaseSeeder(AppUserManager userManager, AppRoleManager roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public async Task Seed()
        {
            if (!_roleManager.Roles.AsNoTracking().Any(r => r.Name.Equals("admin")))
            {
                var role = new Role
                {
                    Name = "admin",
                    DisplayName = "Admin"
                };
                await _roleManager.CreateAsync(role);
            }

            if (!_roleManager.Roles.AsNoTracking().Any(r => r.Name.Equals("user")))
            {
                var role = new Role
                {
                    Name = "user",
                    DisplayName = "User"
                };
                await _roleManager.CreateAsync(role);
            }


            if (!_userManager.Users.AsNoTracking().Any(u => u.UserName.Equals("admin")))
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@keystone.com",
                    PhoneNumberConfirmed = true,
                    FamilyName = "Super",
                    Name = "Admin",
                };

                await _userManager.CreateAsync(user, "qwerty123!");
                await _userManager.AddToRoleAsync(user, "admin");
            }

            if (!_userManager.Users.AsNoTracking().Any(u => u.UserName.Equals("johndoe")))
            {
                var user = new User
                {
                    UserName = "johndoe",
                    Email = "jdoe@keystone.com",
                    PhoneNumberConfirmed = true,
                    FamilyName = "Doe",
                    Name = "John",
                };

                await _userManager.CreateAsync(user, "qwerty123!");
                await _userManager.AddToRoleAsync(user, "user");
            }
        }
    }
}
