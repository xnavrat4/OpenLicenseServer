using OpenLicenseServerDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OpenLicenseServerDAL.Data
{
    public static class DataInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            var license = new License() { Id = 1, SerialNumber = Guid.NewGuid() };
            modelBuilder.Entity<License>().HasData(license);
            //User
            var user = new User()
                { Id = 1, Email = "user@email.com", FirstName = "Martin", LastName = "User", PhoneNumber = "+420811895549"};
            var admin = new User()
                { Id = 2, Email = "admin@email.com", FirstName = "Admin", LastName = "Super", PhoneNumber = "+45196485565"};
            modelBuilder.Entity<User>().HasData(user, admin);

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();

            //Seeding a 'Admin' and 'User' role to AspNetRoles table
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                    {Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole
                    {Id = "2c5e174e-3b0e-446f-86af-483d56fd8321", Name = "User", NormalizedName = "USER"});

            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                    UserName = "MartinUser",
                    NormalizedUserName = "MARTINUSER",
                    Email = "user@email.com",
                    NormalizedEmail = "USER@EMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "user1234")
                },
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048egg5", // primary key
                    UserName = "AdminSuper",
                    NormalizedUserName = "ADMINSUPER",
                    Email = "admin@email.com",
                    NormalizedEmail = "ADMIN@EMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "admin1234")
                }
            );

            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048egg5"
                },
                
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd8321",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );
            
        }
    }
}
