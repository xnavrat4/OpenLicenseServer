using AutoMapper;
using Infrastructure.EFCore.Query;
using Infrastructure.UnitOfWork;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.DTOs.License;
using OpenLicenseServerBL.QueryObjects;
using OpenLicenseServerBL.Services;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Facades;

public class LicenseFacade : ILicenseFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILicenseService _licenseService;
    private readonly LicensesQueryObject _licensesQueryObject;
    private readonly LicenseKeyQueryObject _licenseKeyQueryObject;

    public LicenseFacade(ILicenseService licenseService, IUnitOfWork unitOfWork, IMapper mapper, LicensesQueryObject licensesQueryObject, LicenseKeyQueryObject licenseKeyQueryObject)
    {
        _licenseService = licenseService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _licensesQueryObject = licensesQueryObject;
        _licenseKeyQueryObject = licenseKeyQueryObject;
    }

    public async Task<LicenseDto> GetValidateLicense(LicenseValidateDto validateDto)
    {
        //var filterDto = _mapper.Map<LicenseKeyFilterDto>(validateDto);
        var filterDto = new LicenseKeyFilterDto()
        {
            LicenseKey = new Guid(validateDto.LicenseKey), SortCriteria = nameof(License.Id), SortDescending = false
        };
        var queryResult = await _licenseKeyQueryObject.ExecuteQueryAsync(filterDto);
        if (!queryResult.Items.Any())
        {
            throw new Exception($"No license with given serial number ${validateDto.LicenseKey} was found");
        }
        
        var license = queryResult.Items.First();
        if (license.DeviceId == null)
        {
            //License is not assigned to any device
            license.DeviceId = validateDto.DeviceId;
            await _licenseService.UpdateAsync(license);
            return _mapper.Map<LicenseDto>(license);
        }
        //License is assigned to a device
        if (license.DeviceId != validateDto.DeviceId)
        {
            throw new Exception($"License with serial number ${validateDto.LicenseKey} is bound to a different device");
        }

        return _mapper.Map<LicenseDto>(license);

    }

    public async Task<IEnumerable<License>> GetLicenseByLicenseKey(string licenseKey)
    {
        var filterDto = new LicenseKeyFilterDto()
        {
            LicenseKey = new Guid(licenseKey), SortCriteria = nameof(License.Id), SortDescending = false
        };
        var queryResult = await _licenseKeyQueryObject.ExecuteQueryAsync(filterDto);
        return queryResult.Items;
    }

    public async Task<LicenseDto> AssignLicenseToDevice(License license, int deviceId)
    {
        license.DeviceId = deviceId;
        await _licenseService.UpdateAsync(license);
        return _mapper.Map<LicenseDto>(license);
    }

    public LicenseDto ReturnLicenseToDevice(License license)
    {
        return _mapper.Map<LicenseDto>(license);
    }

    public async Task<IEnumerable<LicenseDto>> GetAllLicensesAsync()
    {
        var licenses = await _licenseService.GetAllAsync();
        return _mapper.Map<IEnumerable<LicenseDto>>(licenses);
    }

    public async Task<IEnumerable<LicenseDto>> GetLicensesOfDevice(int deviceId)
    {
        var all = await _licenseService.GetAllAsync();
        var filtered = all.Where(l => l.DeviceId == deviceId);
        return _mapper.Map<IEnumerable<LicenseDto>>(filtered);
    }

    public async Task<QueryResult<LicenseDto>> GetFilteredLicenses(FilterDto filterDto)
    {
        var queryResult = await _licensesQueryObject.ExecuteQueryAsync(filterDto);
        return _mapper.Map<QueryResult<LicenseDto>>(queryResult);
    }
}
