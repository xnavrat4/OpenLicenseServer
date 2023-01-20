using OpenLicenseServerDAL.Models;
using Infrastructure.Repository;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        public IRepository<User> UserRepository { get; }
        public IRepository<Customer> CustomerRepository { get; }
        public IRepository<Device> DeviceRepository { get; }
        public IRepository<HWInfo> HwInfoRepository { get; }
        public IRepository<License> LicenseRepository { get; }  
        

        public Task CommitAsync();
    }
}
