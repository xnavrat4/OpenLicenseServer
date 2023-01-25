using AutoMapper;
using Infrastructure.UnitOfWork;
using OpenLicenseServerBL.DTOs.License;
using OpenLicenseServerBL.Services;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Facades;

public class LicenseFacade : ILicenseFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILicenseService _licenseService;

    public LicenseFacade(ILicenseService licenseService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _licenseService = licenseService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateLicenseAsync(LicenseCreateDto licenseDto)
    {
        var license = _mapper.Map<License>(licenseDto);
        await _licenseService.CreateAsync(license);
    }

    public async Task<LicenseDto?> GetLicenseByIdAsync(int licenseId)
    {
        var license = await _licenseService.GetById(licenseId);
        return _mapper.Map<LicenseDto>(license);
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
}