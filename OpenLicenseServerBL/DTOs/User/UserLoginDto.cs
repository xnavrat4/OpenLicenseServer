using System.ComponentModel.DataAnnotations;
using OpenLicenseServerBL.ValidationAttributes;

namespace OpenLicenseServerBL.DTOs;

public class UserLoginDto
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}