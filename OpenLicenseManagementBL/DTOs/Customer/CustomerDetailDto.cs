using OpenLicenseManagementBL.DTOs.Device;

namespace OpenLicenseManagementBL.DTOs.Customer;

public class CustomerDetailDto : CustomerBaseDto
{
    public CustomerDetailDto()
    {
        Devices = new List<DevicePreviewDto>();
    }
    
    public int Id { get; set; }
    public IList<DevicePreviewDto> Devices { get; set; }
}