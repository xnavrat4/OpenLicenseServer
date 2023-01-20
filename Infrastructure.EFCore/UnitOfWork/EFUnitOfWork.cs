using OpenLicenseServerDAL.Data;
using OpenLicenseServerDAL.Models;
using Infrastructure.EFCore.Repository;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private OLSDbContext _dbContext;

        public EFUnitOfWork(OLSDbContext dbContext, IRepository<User> userRepository, 
            IRepository<Customer> customerRepository, IRepository<Device> deviceRepository, 
            IRepository<HWInfo> hwInfoRepository, IRepository<License> licenseRepository)
        {
            _dbContext = dbContext;
            UserRepository = userRepository;
            CustomerRepository = customerRepository;
            DeviceRepository = deviceRepository;
            HwInfoRepository = hwInfoRepository;
            LicenseRepository = licenseRepository;
        }

        public IRepository<User> UserRepository { get; }
        public IRepository<Customer> CustomerRepository { get; }
        public IRepository<Device> DeviceRepository { get; }
        public IRepository<HWInfo> HwInfoRepository { get; }
        public IRepository<License> LicenseRepository { get; }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
