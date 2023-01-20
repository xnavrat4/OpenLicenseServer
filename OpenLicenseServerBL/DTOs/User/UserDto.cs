namespace OpenLicenseServerBL.DTOs
{
    public class UserDto : UserDtoBase
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
