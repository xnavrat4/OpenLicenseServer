namespace OpenLicenseServerBL.DTOs;

public class ViolationDto
{
    public int ViolationType { get; set; }
    
    public DateTime DateTime { get; set; }
    
    public string FormerValue { get; set; }
    
    public string CurrentValue { get; set; }
}