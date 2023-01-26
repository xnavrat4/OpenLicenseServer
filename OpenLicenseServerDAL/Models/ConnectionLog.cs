using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLicenseServerDAL.Models;

public class ConnectionLog : BaseEntity
{
    public DateTime TimeStamp { get; set; }
    
    public string HWInfoHash { get; set; }
    
    public int? LicenseId { get; set; }
    
    [ForeignKey(nameof(LicenseId))]
    public virtual License License { get; set; }
}