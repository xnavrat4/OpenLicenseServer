using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models;

public class Device : BaseEntity
{
    [MaxLength(64)]
    public string SerialNumber { get; set; }
    
    public virtual List<HWInfo> HwInfos { get; set; }
    
    public virtual List<License> Licenses { get; set; }

}