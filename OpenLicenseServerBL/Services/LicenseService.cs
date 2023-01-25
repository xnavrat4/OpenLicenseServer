using Infrastructure.Repository;
using License = OpenLicenseServerDAL.Models.License;

namespace OpenLicenseServerBL.Services;

public class LicenseService : BaseService<License>
{
    public LicenseService(IRepository<License> repository) : base(repository)
    {
    }
}