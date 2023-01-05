using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OpenLicenseManagementBL.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class EmailAddressStrictAttribute : DataTypeAttribute
{
    private static readonly Regex Regex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public EmailAddressStrictAttribute() : base(DataType.EmailAddress)
    {
    }

    public override bool IsValid(object? value)
    {
        return value == null || value is string stringValue && Regex.Match(stringValue).Length > 0;
    }
}