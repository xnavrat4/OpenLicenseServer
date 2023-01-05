using OpenLicenseManagementBL.Enums;

namespace OpenLicenseManagementBL.DTOs;

public class ViolationDto : BaseDTO
{
    public ViolationType ViolationType { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public string FormerValue { get; set; }
    
    public string CurrentValue { get; set; }
    
    public bool Resolved { get; set; }
    
    public int DeviceId { get; set; }
}