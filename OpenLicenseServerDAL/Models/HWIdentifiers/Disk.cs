using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models.HWIdentifiers;

public class Disk : BaseEntity
{
    [MaxLength(64)]
    public string DiskId { get; set; }
    
    [MaxLength(64)]
    public string SerialNumber { get; set; }
    
    [MaxLength(64)]
    public string Size { get; set; }
    
    public int HWInfoId { get; set; }
    
    [ForeignKey(nameof(HWInfoId))]
    public virtual HWInfo HwInfo { get; set; }
}