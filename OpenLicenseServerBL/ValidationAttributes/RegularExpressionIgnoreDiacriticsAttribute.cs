using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace OpenLicenseServerBL.ValidationAttributes;

/// <summary>
///     Regular expression validation attribute which ignores diacritics
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class RegularExpressionIgnoreDiacriticsAttribute : RegularExpressionAttribute
{
    public RegularExpressionIgnoreDiacriticsAttribute(string pattern) : base(pattern)
    {
    }

    public override bool IsValid(object? value)
    {
        // Convert the value to a string
        var stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);

        // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
        if (string.IsNullOrEmpty(stringValue))
        {
            return true;
        }
        
        return base.IsValid(RemoveDiacritics(stringValue));
    }
    
    private string RemoveDiacritics(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Normalize(NormalizationForm.FormD);
        var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}