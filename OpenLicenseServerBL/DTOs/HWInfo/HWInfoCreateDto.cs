using OpenLicenseServerBL.DTOs.HWInfo;
using OpenLicenseServerBL.DTOs.HWInfo.MBDtos;
using OpenLicenseServerBL.DTOs.HWInfo.OSDtos;
using OpenLicenseServerDAL.Models.HWIdentifiers;

namespace OpenLicenseServerBL.DTOs.HWInfo;

public class HWInfoCreateDto
{
    public HWInfoCreateDto()
    {
        RamModuleDto = new List<RAMMModuleCreateDto>();
        MACAddressList = new List<MACAdressDto>();
        DiskList = new List<DiskDto>();
    }
    public MotherBoardDto MotherBoardDto { get; set; }
    
    public OperatingSystemDto OperatingSystemDto { get; set; }

    public IList<RAMMModuleCreateDto> RamModuleDto { get; set; }
    
    public ProcessorDto ProcessorDto { get; set; }
    
    public IList<MACAdressDto> MACAddressList { get; set; }
    
    public IList<DiskDto> DiskList { get; set; }
}