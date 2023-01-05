using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models.HWIdentifiers;

public class MACAddress : BaseEntity
{
    [MaxLength(32)]
    public string Address { get; set; }
    
    public int HWInfoId { get; set; }
    
    [ForeignKey(nameof(HWInfoId))]
    public virtual HWInfo HwInfo { get; set; }

}