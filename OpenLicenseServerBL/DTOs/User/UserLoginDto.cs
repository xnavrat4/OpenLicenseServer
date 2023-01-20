using System.ComponentModel.DataAnnotations;
using OpenLicenseServerBL.ValidationAttributes;

namespace OpenLicenseServerBL.DTOs;

public class UserLoginDto
{    
    [Required]
    [EmailAddressStrict(ErrorMessage = "Email address is not valid.")]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}