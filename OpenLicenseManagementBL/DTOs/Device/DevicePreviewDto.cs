using OpenLicenseManagementBL.DTOs;

namespace OpenLicenseManagementBL.DTOs.Device;

public class DevicePreviewDto : BaseDTO
{
    public string SerialNumber { get; set; }
    
    public string? AdditionalInfo { get; set; }
    
    public int NumberOfLicenses { get; set; }
}