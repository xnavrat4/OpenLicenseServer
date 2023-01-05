using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenLicenseServerDAL.Models.HWIdentifiers;
using OperatingSystem = OpenLicenseServerDAL.Models.HWIdentifiers.OperatingSystem;

namespace OpenLicenseServerDAL.Models;

public class HWInfo : BaseEntity
{
    public virtual List<MACAddress> MACAddressList { get; set; }

    public virtual List<RAMModule> RAMModuleList { get; set; }

    public virtual List<Disk> DiskList { get; set; }

    public int ProcessorId { get; set; }
    
    [ForeignKey(nameof(ProcessorId))]
    public virtual Processor Processor { get; set; }
    
    public int MotherBoardId { get; set; }
    
    [ForeignKey(nameof(MotherBoardId))]
    public virtual MotherBoard MotherBoard { get; set; }
    
    public int OperatingSystemId { get; set; }
    
    [ForeignKey(nameof(OperatingSystemId))]
    public virtual OperatingSystem OperatingSystem { get; set; }
}