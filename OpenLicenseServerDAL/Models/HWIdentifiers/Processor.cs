using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models.HWIdentifiers;

public class Processor : BaseEntity
{
    [MaxLength(64)]
    public string Type { get; set; }
    
    [MaxLength(64)]
    public string ProcessorId { get; set; }
}