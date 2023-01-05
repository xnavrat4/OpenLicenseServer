using OpenLicenseManagementBL.Enums;

namespace OpenLicenseManagementBL.DTOs.Query;

public class UserStatusFilterDto : FilterDto
{
    public UserStatus UserStatus { get; set; }
}