using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class License : BaseEntity
{
    public Guid LicenseKey { get; set; }
    
    [MaxLength(32)]
    public string ProductName { get; set; }
    
    [MaxLength(512)] 
    public string Parameters { get; set; }

    public bool Revoked { get; set; }
    
    public DateTime ValidFrom { get; set; }
    
    public DateTime ValidTo { get; set; }
    public int? DeviceId { get; set; }
    
    [ForeignKey(nameof(DeviceId))]
    public virtual Device? Device { get; set; }  
}