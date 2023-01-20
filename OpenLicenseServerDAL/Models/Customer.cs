using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class Customer : BaseEntity
{
    [MaxLength(64)]
    public string Name { get; set; }
    
    virtual  public List<Device> Devices { get; set; }

}