using OpenLicenseManagementBL.DTOs.Customer;
using OpenLicenseManagementBL.DTOs.HWInfo;

namespace OpenLicenseManagementBL.DTOs.Device;

public class DeviceValidateDto
{
    public int Id { get; set; }
    
    public string SerialNumber { get; set; }
    
    public string AdditionalInfo { get; set; }
    
    public int HeartbeatFrequency { get; set; }
    
    public bool Validity { get; set; }
    
    public CustomerNameDto Customer { get; set; }
    
    public HWInfoDto HwInfo { get; set; }
}