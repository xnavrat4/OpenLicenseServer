using OpenLicenseManagementBL.Enums;

namespace OpenLicenseManagementBL.DTOs.User;

public class UserFullDto : UserDto
{

    public UserDtoBase? AddedBy { get; set; }
    
    public DateTime? AddedOn { get; set; }
}