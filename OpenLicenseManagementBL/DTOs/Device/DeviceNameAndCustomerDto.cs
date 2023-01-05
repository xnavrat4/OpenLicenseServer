using OpenLicenseManagementBL.DTOs.Customer;

namespace OpenLicenseManagementBL.DTOs.Device;

public class DeviceNameAndCustomerDto : BaseDTO
{
    public string SerialNumber { get; set; }
    
    public DateTime LastOnline { get; set; }
    
    public CustomerNameDto Customer { get; set; }
}