using System.ComponentModel.DataAnnotations;
using OpenLicenseManagementBL.ValidationAttributes;

namespace OpenLicenseManagementBL.DTOs.User;

public class UserLoginDto
{    
    [Required]
    [EmailAddressStrict(ErrorMessage = "Email address is not valid.")]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}