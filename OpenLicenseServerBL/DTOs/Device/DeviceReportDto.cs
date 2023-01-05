using OpenLicenseServerBL.DTOs.License;

namespace OpenLicenseServerBL.DTOs.Device;

public class DeviceReportDto
{
    public int Id { get; set; }

    public DateTime ServerTime { get; set; }
    
    //in hours
    public int HeartbeatFrequency { get; set; }

    public virtual List<LicenseDto>? Licenses { get; set; }
    
    public virtual List<ViolationDto>? Violations { get; set; }
}