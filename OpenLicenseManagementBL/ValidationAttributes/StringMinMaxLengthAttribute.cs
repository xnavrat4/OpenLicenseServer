using System.ComponentModel.DataAnnotations;

namespace OpenLicenseManagementBL.ValidationAttributes;

/// <summary>
///     Validation attribute to assert a string property, field or parameter is within min/max length
///     Removes the start of default error message: 'The field '...
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class StringMinMaxLengthAttribute : StringLengthAttribute
{
    private const string PrefixToRemove = "The field ";

    public StringMinMaxLengthAttribute(int maximumLength) : base(maximumLength)
    {
    }

    public override string FormatErrorMessage(string name)
    {
        var baseMessage = base.FormatErrorMessage(name);

        return baseMessage.StartsWith(PrefixToRemove) 
            ? baseMessage[PrefixToRemove.Length..] 
            : baseMessage;
    }
}