using OpenLicenseServerBL.DTOs.HWInfo;

namespace OpenLicenseServerBL.DTOs.Device;

public class DeviceRegisterDto
{
    public HWInfoCreateDto HwInfo { get; set; }
    
    public string PublicKey { get; set; }
}