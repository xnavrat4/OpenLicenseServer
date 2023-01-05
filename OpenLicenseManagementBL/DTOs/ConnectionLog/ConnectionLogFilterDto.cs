using OpenLicenseManagementBL.DTOs.Query;

namespace OpenLicenseManagementBL.DTOs.ConnectionLog;

public class ConnectionLogFilterDto : FilterDto
{
    public int DeviceId { get; set; }
}