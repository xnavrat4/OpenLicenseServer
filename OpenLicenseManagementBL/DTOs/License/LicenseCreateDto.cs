using OpenLicenseManagementBL.DTOs.Device;

namespace OpenLicenseManagementBL.DTOs.License;

public class LicenseCreateDto
{
    public string ProductName { get; set; }
    
    public string Parameters { get; set; }

    public DateTime ValidFrom { get; set; }
    
    public DateTime ValidTo { get; set; }
}