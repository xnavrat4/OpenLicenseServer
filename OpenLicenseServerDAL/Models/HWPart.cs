using System.ComponentModel.DataAnnotations;

namespace OpenLicenseServerDAL.Models;

public abstract class HWPart : BaseEntity
{
    [MaxLength(64)]
    public string? SerialNumber { get; set; }
}