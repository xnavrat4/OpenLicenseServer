using OpenLicenseManagementBL.DTOs.Customer;
using OpenLicenseManagementBL.DTOs.HWInfo;
using OpenLicenseManagementBL.DTOs.License;

namespace OpenLicenseManagementBL.DTOs.Device;

public class DeviceDto
{
    public int Id { get; set; }

    public string SerialNumber { get; set; }

    public string? AdditionalInfo { get; set; }
    
    public DateTime LastOnline { get; set; }

    public CustomerNameDto Customer { get; set; }
    
    public HWInfoDto HwInfo { get; set; }

    public int NumberOfLicenses { get; set; }
}