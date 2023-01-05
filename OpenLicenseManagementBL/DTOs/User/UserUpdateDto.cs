using System.ComponentModel.DataAnnotations;
using OpenLicenseManagementBL.ValidationAttributes;

namespace OpenLicenseManagementBL.DTOs.User
{
    public class UserUpdateDto
    {
        [Required]
        public int? Id { get; set; }
        
        [Required]
        [StringMinMaxLength(20, MinimumLength = 1)]
        [RegularExpressionIgnoreDiacritics(@"^[a-zA-Z]+$",
            ErrorMessage =
                "First name must contain only letters!")]
        public string FirstName { get; set; }        
        
        [Required]
        [StringMinMaxLength(20, MinimumLength = 1)]
        [RegularExpressionIgnoreDiacritics(@"^[a-zA-Z]+$",
            ErrorMessage =
                "Last name must contain only letters!")]
        public string LastName { get; set; }
        
        [Required]
        [RegularExpression(@"^\+\d{3}\d{6,12}$",
            ErrorMessage =
                "Phone number must be inputted including an area code (e.g., +420111222333).")]
        public string PhoneNumber { get; set; }
    }
}
