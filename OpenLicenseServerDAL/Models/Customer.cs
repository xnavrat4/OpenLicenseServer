using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class Customer : BaseEntity
{
    [MaxLength(64)]
    public string Name { get; set; }
    
    [MaxLength(64)]
    public string City { get; set; }
    
    [MaxLength(64)]
    public string Country { get; set; }
    
    public virtual List<Device>? Devices { get; set; }

}