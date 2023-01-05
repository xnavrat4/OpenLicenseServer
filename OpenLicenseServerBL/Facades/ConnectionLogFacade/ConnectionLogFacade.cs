using AutoMapper;
using Infrastructure.UnitOfWork;
using OpenLicenseServerBL.DTOs.ConnectionLog;
using OpenLicenseServerBL.Services;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Facades;

public class ConnectionLogFacade : IConnectionLogFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConnectionLogService _connectionLogService;

    public ConnectionLogFacade(IMapper mapper, IUnitOfWork unitOfWork, IConnectionLogService connectionLogService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _connectionLogService = connectionLogService;
    }
    

    public async Task CreateLogAsync(ConnectionLogCreateDto createDto)
    {
        var connectionLog = _mapper.Map<ConnectionLog>(createDto);
        await _connectionLogService.CreateAsync(connectionLog);
    }
}