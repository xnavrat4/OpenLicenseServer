using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class Device : BaseEntity
{
    [MaxLength(64)]
    public string SerialNumber { get; set; }
    
    [MaxLength(1024)]
    public string? AdditionalInfo { get; set; }

    public int? CustomerId { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer? Customer { get; set; }  
    
    public virtual List<HWInfo> HwInfos { get; set; }
    
    public virtual List<License> Licenses { get; set; }

}