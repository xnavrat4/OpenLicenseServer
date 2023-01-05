using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class Violation : BaseEntity
{
    public int ViolationType { get; set; }
    
    public DateTime DateTime { get; set; }
    
    [MaxLength(1024)]
    public string FormerValue { get; set; }
    
    [MaxLength(1024)]
    public string CurrentValue { get; set; }
    
    public bool Resolved { get; set; }
    
    public int DeviceId { get; set; }
    
    [ForeignKey(nameof(DeviceId))]
    public virtual Device Device { get; set; }  
}