using OpenLicenseManagementBL.DTOs.Query;

namespace OpenLicenseServerBL.DTOs.Device;

public class DeviceViolationsFilterDto : FilterDto
{
    public int DeviceId { get; set; }
}