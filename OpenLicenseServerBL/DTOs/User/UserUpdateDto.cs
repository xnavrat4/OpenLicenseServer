using System.ComponentModel.DataAnnotations;
using OpenLicenseServerBL.ValidationAttributes;

namespace OpenLicenseServerBL.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public int? Id { get; set; }
        
        [Required]
        [StringMinMaxLength(20, MinimumLength = 2)]
        [RegularExpressionIgnoreDiacritics(@"^[A-Za-z .'-]+$",
            ErrorMessage =
                "First name must contain only letters and following characters: .'-")]
        public string FirstName { get; set; }        
        
        [Required]
        [StringMinMaxLength(20, MinimumLength = 2)]
        [RegularExpressionIgnoreDiacritics(@"^[A-Za-z .'-]+$",
            ErrorMessage =
                "Last name must contain only letters and following characters: .'-")]
        public string LastName { get; set; }
        
        [Required]
        [RegularExpression(@"^\+\d{3}\d{6,12}$",
            ErrorMessage =
                "Phone number must start with '+' followed by digits only. Required number of digits is 9-15. Supported format: +421911222333.")]
        public string PhoneNumber { get; set; }
    }
}
