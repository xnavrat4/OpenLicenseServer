using OpenLicenseManagementBL.DTOs.ConnectionLog;
using OpenLicenseManagementBL.DTOs.Customer;
using OpenLicenseManagementBL.DTOs.HWInfo;
using OpenLicenseManagementBL.DTOs.License;

namespace OpenLicenseManagementBL.DTOs;

public class DeviceDetailDto
{
    public DeviceDetailDto()
    {
        Licenses = new List<LicenseDto>();
        Violations = new List<ViolationDto>();
    }

    public int Id { get; set; }
    
    public string? SerialNumber { get; set; }
    
    public string? AdditionalInfo { get; set; }
    
    public string HWInfoHash { get; set; }
    
    public int HeartbeatFrequency { get; set; }
    
    public bool Activated { get; set; }
    
    public CustomerNameDto Customer { get; set; }  
    
    public HWInfoDto HwInfo { get; set; }
    
    public List<LicenseDto> Licenses { get; set; }
    
    public List<ConnectionLogDto> ConnectionLogs { get; set; }
    
    public List<ViolationDto>? Violations { get; set; }
}