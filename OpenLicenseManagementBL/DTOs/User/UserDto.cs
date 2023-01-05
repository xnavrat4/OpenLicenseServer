using OpenLicenseManagementBL.Enums;

namespace OpenLicenseManagementBL.DTOs.User
{
    public class UserDto : UserDtoBase
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        
        public UserStatus UserStatus { get; set; }
    }
}
