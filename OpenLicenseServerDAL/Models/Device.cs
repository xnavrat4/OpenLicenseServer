using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class Device : BaseEntity
{
    [MaxLength(64)]
    public string? SerialNumber { get; set; }
    
    [MaxLength(1024)]
    public string? AdditionalInfo { get; set; }
    
    [MaxLength(90)]
    public string HWInfoHash { get; set; }
    
    public bool Activated { get; set; }
    
    public bool Validated { get; set; }
    
    public string PublicKey { get; set; }
    
    public DateTime? LastOnline { get; set; }
    
    //in hours
    public int HeartbeatFrequency { get; set; }

    public int? CustomerId { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }  
    
    public int? HWInfoId { get; set; }
    
    [ForeignKey(nameof(HWInfoId))]
    public virtual HWInfo HwInfo { get; set; }
    
    public virtual List<License>? Licenses { get; set; }
    
    public virtual List<ConnectionLog> ConnectionLogs { get; set; }
    
    public virtual List<Violation>? Violations { get; set; }

}