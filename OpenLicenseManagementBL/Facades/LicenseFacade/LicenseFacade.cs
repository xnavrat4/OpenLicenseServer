using AutoMapper;
using Infrastructure.EFCore.Query;
using Infrastructure.UnitOfWork;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.License;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.QueryObjects;
using OpenLicenseManagementBL.Services;
using OpenLicenseServerBL.DTOs.Device;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Facades;

public class LicenseFacade : ILicenseFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILicenseService _licenseService;
    private readonly LicensesQueryObject _licensesQueryObject;

    public LicenseFacade(ILicenseService licenseService, IUnitOfWork unitOfWork, IMapper mapper, LicensesQueryObject licensesQueryObject)
    {
        _licenseService = licenseService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _licensesQueryObject = licensesQueryObject;
    }

    public async Task<LicenseDto> CreateLicenseAsync(LicenseCreateDto licenseDto)
    {
        var license = _mapper.Map<License>(licenseDto);
        license.LicenseKey = Guid.NewGuid();
        await _licenseService.CreateAsync(license);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<LicenseDto>(license);
    }

    public async Task<LicenseDetailDto?> GetLicenseByIdAsync(int licenseId)
    {
        var license = await _licenseService.GetById(licenseId);
        return _mapper.Map<LicenseDetailDto>(license);
    }

    public async Task UpdateLicenseAsync(LicenseDto licenseDto)
    {
        var license = _mapper.Map<License>(licenseDto);
        await _licenseService.UpdateAsync(license);
    }

    public async Task<IEnumerable<LicenseDto>> GetAllLicensesAsync()
    {
        var licenses = await _licenseService.GetAllAsync();
        return _mapper.Map<IEnumerable<LicenseDto>>(licenses);
    }

    public async Task DeleteLicenseAsync(int licenseId)
    {
        await _licenseService.DeleteAsync(licenseId);
    }

    public async Task<QueryResult<LicenseDto>> GetFilteredLicenses(FilterDto filterDto)
    {
        var queryResult = await _licensesQueryObject.ExecuteQueryAsync(filterDto);
        return _mapper.Map<QueryResult<LicenseDto>>(queryResult);
    }
}
