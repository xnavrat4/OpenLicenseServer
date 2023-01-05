using OpenLicenseManagementBL.DTOs.Device;

namespace OpenLicenseManagementBL.DTOs.License;

public class LicenseDetailDto : BaseDTO
{
    public Guid LicenseKey { get; set; }
    
    public string ProductName { get; set; }

    public string Parameters { get; set; }

    public bool Revoked { get; set; }
    
    public DateTime ValidFrom { get; set; }
    
    public DateTime ValidTo { get; set; }
    
    public DeviceNameAndCustomerDto? Device { get; set; }
}