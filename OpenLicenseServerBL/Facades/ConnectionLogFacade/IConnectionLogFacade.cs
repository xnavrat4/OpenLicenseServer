using OpenLicenseServerBL.DTOs.ConnectionLog;

namespace OpenLicenseServerBL.Facades;

public interface IConnectionLogFacade
{
    Task CreateLogAsync(ConnectionLogCreateDto createDto);
}