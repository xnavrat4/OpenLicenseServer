using System.Globalization;
using AutoMapper;
using Infrastructure.EFCore.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.DTOs.ConnectionLog;
using OpenLicenseServerBL.DTOs.Device;
using OpenLicenseServerBL.DTOs.HWInfo;
using OpenLicenseServerBL.DTOs.HWInfo.MBDtos;
using OpenLicenseServerBL.DTOs.HWInfo.OSDtos;
using OpenLicenseServerBL.DTOs.License;
using OpenLicenseServerDAL.Models;
using OpenLicenseServerDAL.Models.HWIdentifiers;
using OperatingSystem = OpenLicenseServerDAL.Models.HWIdentifiers.OperatingSystem;

namespace OpenLicenseServerBL.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Licenses
        CreateMap<LicenseDto, License>().ReverseMap();

        CreateMap<LicenseKeyFilterDto, LicenseValidateDto>()
            .AfterMap((d, s) => d.LicenseKey = new Guid(s.LicenseKey))
            .AfterMap((d, s) => d.SortCriteria = nameof(License.Id))
            .AfterMap((d, s) => d.SortDescending = false);

        CreateMap<QueryResult<License>, QueryResult<LicenseDto>>().ReverseMap();

        //HW Parts
        CreateMap<MotherBoardDto, MotherBoard>().ReverseMap();
        CreateMap<OperatingSystemDto, OperatingSystem>().ReverseMap();
        CreateMap<RAMMModuleCreateDto, RAMModule>().ReverseMap();
        CreateMap<ProcessorDto, Processor>().ReverseMap();
        CreateMap<MACAdressDto, MACAddress>().ReverseMap();
        CreateMap<DiskDto, Disk>().ReverseMap();

        //HW Info
        CreateMap<HWInfoDto, HWInfo>().ReverseMap();
        CreateMap<HWInfoCreateDto, HWInfo>()
            .ForMember(dest => dest.MotherBoard, opt => opt.MapFrom(s => s.MotherBoardDto))
            .ForMember(dest => dest.OperatingSystem, opt => opt.MapFrom(s => s.OperatingSystemDto))
            .ForMember(dest => dest.RAMModuleList, opt => opt.MapFrom(s => s.RamModuleDto))
            .ForMember(dest => dest.Processor, opt => opt.MapFrom(s => s.ProcessorDto))
            .ForMember(dest => dest.MACAddressList, opt => opt.MapFrom(s => s.MACAddressList))
            .ForMember(dest => dest.DiskList, opt => opt.MapFrom(s => s.DiskList));
        CreateMap<HWInfo, HWInfoCreateDto>()
            .ForMember(dest => dest.MotherBoardDto, opt => opt.MapFrom(s => s.MotherBoard))
            .ForMember(dest => dest.OperatingSystemDto, opt => opt.MapFrom(s => s.OperatingSystem))
            .ForMember(dest => dest.RamModuleDto, opt => opt.MapFrom(s => s.RAMModuleList))
            .ForMember(dest => dest.ProcessorDto, opt => opt.MapFrom(s => s.Processor))
            .ForMember(dest => dest.MACAddressList, opt => opt.MapFrom(s => s.MACAddressList))
            .ForMember(dest => dest.DiskList, opt => opt.MapFrom(s => s.DiskList));


        //Devices
        CreateMap<Device, DeviceRegisterDto>().ReverseMap();
        CreateMap<Device, DeviceReportDto>()
            .ForMember(dest => dest.Violations,
                opt => opt.MapFrom(s =>
                    (s.Violations.Any() ? s.Violations.Where(v => !v.Resolved).ToList() : new List<Violation>())))
            .AfterMap((s, d) => d.ServerTime = DateTime.UtcNow);
        
        //Violations
        CreateMap<Violation, ViolationDto>().ReverseMap();
        CreateMap<ConnectionLog, ConnectionLogCreateDto>().ReverseMap();
    }
}