using OpenLicenseManagementBL.Enums;

namespace OpenLicenseManagementBL.DTOs.User;

public class UserValidateDto : BaseDTO
{
    public UserStatus UserStatus { get; set; }
    
    public int AddedById { get; set; }
    
}