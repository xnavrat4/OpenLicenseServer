using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models.HWIdentifiers;

public class OperatingSystem : BaseEntity
{
    [MaxLength(64)]
    public string OSType { get; set; }
    
    [MaxLength(64)]
    public string MachineId { get; set; }
}