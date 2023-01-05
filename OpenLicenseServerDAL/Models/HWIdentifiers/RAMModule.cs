using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models.HWIdentifiers;

public class RAMModule : BaseEntity
{
    [MaxLength(64)]
    public string PartNumber { get; set; }

    [MaxLength(64)]
    public string SerialNumber { get; set; }
    
    [MaxLength(64)]
    public string Size { get; set; }
}