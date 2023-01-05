using OpenLicenseManagementBL.DTOs.HWInfo.MBDtos;
using OpenLicenseManagementBL.DTOs.HWInfo.OSDtos;
using OpenLicenseManagementBL.DTOs.HWInfo.ProcessorDtos;
using OpenLicenseManagementBL.DTOs.HWInfo.RAMDto;
using OpenLicenseServerDAL.Models.HWIdentifiers;

namespace OpenLicenseManagementBL.DTOs.HWInfo;

public class HWInfoDto
{
    public HWInfoDto()
    {
        RamModuleList = new List<RAMMModuleDto>();
        MACAddressList = new List<MACAdressDto>();
        DiskList = new List<DiskDto>();
    }
    public MotherBoardDto MotherBoard { get; set; }
    
    public OperatingSystemDto OperatingSystem { get; set; }

    public IList<RAMMModuleDto> RamModuleList { get; set; }
    
    public ProcessorDto Processor { get; set; }
    
    public IList<MACAdressDto> MACAddressList { get; set; }
    
    public IList<DiskDto> DiskList { get; set; }
}