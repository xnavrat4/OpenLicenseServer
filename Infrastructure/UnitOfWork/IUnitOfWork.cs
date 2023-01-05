using OpenLicenseServerDAL.Models;
using Infrastructure.Repository;
//taken from course FI:PV179 materials
namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        public IRepository<User> UserRepository { get; }
        public IRepository<Customer> CustomerRepository { get; }
        public IRepository<Device> DeviceRepository { get; }
        public IRepository<HWInfo> HwInfoRepository { get; }
        public IRepository<License> LicenseRepository { get; }  
        
        public IRepository<ConnectionLog> ConnectionLogRepository { get; }
        public IRepository<Violation> ViolationRepository { get; }


        public Task CommitAsync();
    }
}
