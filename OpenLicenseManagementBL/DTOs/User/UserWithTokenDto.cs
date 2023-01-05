namespace OpenLicenseManagementBL.DTOs.User;

public class UserWithTokenDto : UserDtoBase
{
    public string Email { get; set; }
    public string Token { get; set; }
    public bool IsAdmin { get; set; }
}