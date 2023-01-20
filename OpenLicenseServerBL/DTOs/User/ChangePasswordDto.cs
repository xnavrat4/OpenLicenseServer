using System.ComponentModel.DataAnnotations;
using OpenLicenseServerBL.ValidationAttributes;

namespace OpenLicenseServerBL.DTOs;

public class ChangePasswordDto
{
    [Required]
    public int? UserId { get; set; }
    
    [Required]
    [Password("Old password")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }
    
    [Required]
    [Password("New password")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}