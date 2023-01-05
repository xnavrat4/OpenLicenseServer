using System.ComponentModel.DataAnnotations.Schema;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerDAL.Models;

public class Challenge : BaseEntity
{
    
    public int DeviceId { get; set; }
    
    [ForeignKey(nameof(DeviceId))]
    public virtual Device Device { get; set; }

    public int Value { get; set; }

}