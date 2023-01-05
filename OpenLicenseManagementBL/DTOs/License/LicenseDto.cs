using OpenLicenseManagementBL.DTOs.Device;

namespace OpenLicenseManagementBL.DTOs.License;

public class LicenseDto : BaseDTO
{
    public Guid LicenseKey { get; set; }
    
    public DeviceNameDto? Device { get; set; }
    
    public string ProductName { get; set; }
    
    public string Parameters { get; set; }

    public bool Revoked { get; set; }
    
    public DateTime ValidFrom { get; set; }
    
    public DateTime ValidTo { get; set; }
}