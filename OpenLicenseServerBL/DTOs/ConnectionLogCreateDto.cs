using OpenLicenseBL.Utils;

namespace OpenLicenseServerBL.DTOs.ConnectionLog;

public class ConnectionLogCreateDto
{
    public int DeviceId { get; set; }
    
    public DateTime SystemTime { get; set; }
    
    public RequestStateEnum Result { get; set; }
    public ConnectionLogEnum Type { get; set; }
    
    public Guid LicenseKey { get; set; } 
}