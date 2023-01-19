using System.ComponentModel.DataAnnotations;

namespace FoodliveryDAL.Models;

public class Device : BaseEntity
{
    [MaxLength(64)]
    public string SerialNumber { get; set; }
    
    public virtual List<HWInfo> HwInfos { get; set; }
    
    public virtual List<License> Licenses { get; set; }

}