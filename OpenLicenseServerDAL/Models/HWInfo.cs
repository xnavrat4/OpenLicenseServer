using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models;

public class HWInfo : BaseEntity
{
    public int DeviceID { get; set; }
    
    public virtual Device Device { get; set; }

        [MaxLength(64)]
    public string MACAddress { get; set; }

    [MaxLength(64)]
    public string DiskSerialNumber { get; set; }
    
    [MaxLength(64)]
    public string RAMSize { get; set; }
    
    [MaxLength(64)]
    public string CPUType { get; set; }
    
    [MaxLength(64)]
    public string MotherBoardType { get; set; }
    [MaxLength(64)]
    public string MachineID { get; set; }
}