using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models.HWIdentifiers;

public class MotherBoard : BaseEntity
{
    [MaxLength(64)]
    public string SerialNumber { get; set; }
    
    [MaxLength(64)]
    public string ProductName { get; set; }
    
    [MaxLength(64)]
    public string Manufacturer { get; set; }
}