using OpenLicenseServerBL.DTOs;

namespace OpenLicenseBL.DTOs;

public class ReportRequestDto
{
    public ReportRequestDto()
    {
        Violations = new List<ViolationDto>();
    }

    public int DeviceId { get; set; }
    public List<ViolationDto> Violations { get; set; }
}