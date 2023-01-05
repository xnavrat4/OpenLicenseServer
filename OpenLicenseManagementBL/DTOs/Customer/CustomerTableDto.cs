using OpenLicenseManagementBL.DTOs.Device;

namespace OpenLicenseManagementBL.DTOs.Customer;

public class CustomerTableDto : CustomerBaseDto
{
    public int Id { get; set; }
    
    public int NumberOfDevices { get; set; }
}