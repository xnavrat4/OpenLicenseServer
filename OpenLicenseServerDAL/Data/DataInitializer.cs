using OpenLicenseServerDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenLicenseServerDAL.Models.HWIdentifiers;
using OperatingSystem = OpenLicenseServerDAL.Models.HWIdentifiers.OperatingSystem;

namespace OpenLicenseServerDAL.Data
{
    public static class DataInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            /*
            var customer = new Customer() {Id = 1, Name = "Daite s.r.o", City = "Brno", Country = "Czech republic" };
            modelBuilder.Entity<Customer>().HasData(customer);

            var p = new Processor() { Id = 1, ProcessorId = "fake", Type = "I7" };
            modelBuilder.Entity<Processor>().HasData(p);

            var os = new OperatingSystem() { Id = 1, OSType = "Windows", MachineId = "fake" };
            modelBuilder.Entity<OperatingSystem>().HasData(os);

            var mb = new MotherBoard()
                { Id = 1, Manufacturer = "Gigabyte", SerialNumber = "fakse", ProductName = "X399" };
            modelBuilder.Entity<MotherBoard>().HasData(mb);
            var ram = new RAMModule() { Id = 1, SerialNumber = "fakeSN", PartNumber = "1", Size = "16" };
            
            modelBuilder.Entity<RAMModule>().HasData(ram);

            for (var i = 0; i < 10; i++)
            {
                var hwInfo = new HWInfo()
                {
                    ProcessorId = 1, Id = 1 + i, OperatingSystemId = 1, MotherBoardId = 1
                };
            
                var mac = new MACAddress() { Id = 1 + i, Address = $"fakeAdress{i}", HWInfoId = i+1};
                modelBuilder.Entity<MACAddress>().HasData(mac);
                
                modelBuilder.Entity<HWInfo>().HasData(hwInfo);
                var c = new Customer() { Id = i + 2, Name = $"Daite{i}", City = $"Brno{i}", Country = $"Cz{i}" };
                modelBuilder.Entity<Customer>().HasData(c);
                var d = new Device() { Id = i + 1, HWInfoId = i+ 1, CustomerId = i + 1, AdditionalInfo = $"This is {i + 1}th comment", 
                    SerialNumber = $"Playoutv{i}", HWInfoHash = "tis an output of hashing function", Activated = true, PublicKey = "a"};
                modelBuilder.Entity<Device>().HasData(d);
                var license = new License() { Id = i+ 1, LicenseKey = Guid.NewGuid(), DeviceId = i + 1, ProductName = "Playout", Parameters = $"{i}HD"};
                modelBuilder.Entity<License>().HasData(license);
            }
            */
            
            //User
            /*
            var user = new User() { Id = 2, Email = "user@email.com", FirstName = "Martin", 
                LastName = "User", PhoneNumber = "+420811895549", UserStatus = 0 };
            modelBuilder.Entity<User>().HasData(user);
            */
            var admin = new User() { Id = 1, Email = "admin@email.com", FirstName = "Admin", 
                LastName = "Super", PhoneNumber = "+45196485565", UserStatus = 1 };
            modelBuilder.Entity<User>().HasData(admin);

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();

            //Seeding a 'Admin' and 'User' role to AspNetRoles table
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
                    {Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "ADMIN"}); 
            
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
                    {Id = "2c5e174e-3b0e-446f-86af-483d56fd8321", Name = "User", NormalizedName = "USER"});
            
            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<IdentityUser>().HasData(
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
            
            /*
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
            */
            //Seeding the relation between our user and role to AspNetUserRoles table
            /*
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd8321",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            );
            */
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048egg5"
                }
            );
            
        }
    }
}
