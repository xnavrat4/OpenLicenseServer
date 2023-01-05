using System.Security.Cryptography;
using System.Text;
using OpenLicenseServerBL.DTOs.HWInfo;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL;

public static class DeviceHasher
{
    public static string Hash(HWInfoCreateDto hwInfo)
    {
        var processorString = $"Processor:{hwInfo.ProcessorDto.Type}{hwInfo.ProcessorDto.ProcessorId}";
        var ramString = "RAMs:";
        var osString = "OS:" + hwInfo.OperatingSystemDto.OSType + hwInfo.OperatingSystemDto.MachineId;
        var mbString = "MB:" + hwInfo.MotherBoardDto.Manufacturer + hwInfo.MotherBoardDto.ProductName + hwInfo.MotherBoardDto.SerialNumber;
        foreach (var ram in hwInfo.RamModuleDto)
        {
            ramString += (ram.PartNumber + ram.SerialNumber + ram.Size);
        }
        var macString = "MACs";
        foreach (var address in hwInfo.MACAddressList)
        {
            macString += address.Address;
        }

        var stringToHash = osString + processorString + mbString + ramString + macString;
        var bytes = Encoding.UTF8.GetBytes(stringToHash);
        var a = SHA512.HashData(bytes);

        var hash = Convert.ToBase64String(a);

        return hash;
    }
}