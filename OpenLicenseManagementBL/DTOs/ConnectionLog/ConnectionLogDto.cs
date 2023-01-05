using OpenLicenseManagementBL.Enums;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.DTOs.ConnectionLog;

public class ConnectionLogDto : BaseDTO
{
    public int DeviceId { get; set; }
    
    public DateTime SystemTime { get; set; }
    
    public RequestStateEnum Result { get; set; }
    public ConnectionLogEnum Type { get; set; }
    
    public Guid LicenseKey { get; set; } 
}