using System.ComponentModel.DataAnnotations;

namespace OpenLicenseManagementBL.ValidationAttributes;

public class PasswordAttribute : RegularExpressionAttribute
{
    private new const string Pattern = @"^[A-Za-z0-9]{8,24}$";
    private const string MessageFormat =
        "{0} must contain only letters or digits. The length must be in interval 8-24 characters.";
    
    public PasswordAttribute(string fieldName) : base(Pattern)
    {
        ErrorMessage = string.Format(MessageFormat, fieldName);
    }
}