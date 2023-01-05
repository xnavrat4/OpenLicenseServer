using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class ConnectionLog : BaseEntity
{
    
    public DateTime SystemTime { get; set; }
    
    public int Result { get; set; }
    
    public int Type { get; set; }
    
    public Guid LicenseKey { get; set; } 
    public int DeviceId { get; set; }
    
    [ForeignKey(nameof(DeviceId))]
    public virtual Device Device { get; set; }
}