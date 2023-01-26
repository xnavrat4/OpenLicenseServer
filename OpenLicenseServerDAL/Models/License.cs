using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class License : BaseEntity
{
    public Guid SerialNumber { get; set; }
    
    public int LicenseType { get; set; }   
    
    public int Status { get; set; }
    
    public DateTime ValidFrom { get; set; }
    
    public DateTime ValidTo { get; set; }
    public int? DeviceId { get; set; }
    
    [ForeignKey(nameof(DeviceId))]
    public virtual Device? Customer { get; set; }  
}