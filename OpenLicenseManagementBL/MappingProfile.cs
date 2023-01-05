using System.Globalization;
using AutoMapper;
using Infrastructure.EFCore.Query;
//using Mysqlx.Crud;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.ConnectionLog;
using OpenLicenseManagementBL.DTOs.Customer;
using OpenLicenseManagementBL.DTOs.Device;
using OpenLicenseManagementBL.DTOs.HWInfo;
using OpenLicenseManagementBL.DTOs.HWInfo.MBDtos;
using OpenLicenseManagementBL.DTOs.HWInfo.OSDtos;
using OpenLicenseManagementBL.DTOs.HWInfo.ProcessorDtos;
using OpenLicenseManagementBL.DTOs.HWInfo.RAMDto;
using OpenLicenseManagementBL.DTOs.License;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.DTOs.User;
using OpenLicenseManagementBL.Enums;
using OpenLicenseServerDAL.Models;
using OpenLicenseServerDAL.Models.HWIdentifiers;
using CustomerNameDto = OpenLicenseManagementBL.DTOs.Customer.CustomerNameDto;
using OperatingSystem = OpenLicenseServerDAL.Models.HWIdentifiers.OperatingSystem;

namespace OpenLicenseManagementBL.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Licenses
        CreateMap<LicenseDto, License>().ReverseMap();
        CreateMap<LicenseCreateDto, License>()
            .ForMember(d => d.Device, opt => opt.Ignore())
            .AfterMap((s, d) => d.LicenseKey = Guid.NewGuid());
        CreateMap<License, LicenseCreateDto>();
        
        CreateMap<LicenseKeyFilterDto, LicenseValidateDto>()
            .AfterMap((d, s) => d.LicenseKey = new Guid(s.LicenseKey))
            .AfterMap((d, s) => d.SortCriteria = nameof(License.Id))
            .AfterMap((d, s) => d.SortDescending = false);

        CreateMap<QueryResult<License>, QueryResult<LicenseDto>>().ReverseMap();
        CreateMap<LicenseDetailDto, License>().ReverseMap();
        
        //HW Parts
        CreateMap<MotherBoardDto, MotherBoard>().ReverseMap();
        CreateMap<OperatingSystemDto, OperatingSystem>().ReverseMap();
        CreateMap<RAMMModuleDto, RAMModule>().ReverseMap();
        CreateMap<ProcessorDto, Processor>().ReverseMap();
        CreateMap<MACAdressDto, MACAddress>().ReverseMap();
        CreateMap<DiskDto, Disk>().ReverseMap();

        //HW Info
        CreateMap<HWInfoDto, HWInfo>().ReverseMap();

        // Connection Logs
        CreateMap<ConnectionLog, ConnectionLogDto>().ReverseMap();
        CreateMap<QueryResult<ConnectionLog>, QueryResultDto<ConnectionLogDto>>().ReverseMap();
        
        //Users
        CreateMap<User, UserDtoBase>();
        CreateMap<User, UserCreateDto>();
        CreateMap<UserCreateDto, User>()
            .AfterMap((s,d) => d.UserStatus = 0);
        CreateMap<User, UserValidateDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
        CreateMap<User, UserFullDto>()
            .ForMember(d => d.UserStatus, opt => opt.MapFrom(s => (UserStatus)s.UserStatus));
        CreateMap<QueryResult<User>, QueryResultDto<UserFullDto>>();
        CreateMap<UserDto, UserWithTokenDto>();

        //Customers
        CreateMap<Customer, CustomerTableDto>()
            .AfterMap(((customer, dto) => dto.NumberOfDevices = customer.Devices != null && customer.Devices.Any() ? customer.Devices.Count : 0));
        CreateMap<Customer, CustomerCreateDto>().ReverseMap();
        CreateMap<Customer, CustomerNameDto>().ReverseMap();
        CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
        CreateMap<Customer, CustomerDetailDto>().ReverseMap();
        CreateMap<Customer, CustomerBaseDto>().ReverseMap();
        CreateMap<QueryResult<Customer>, QueryResult<CustomerTableDto>>();
        
        //Devices
        CreateMap<Device, DeviceDto>()
            .ForMember(dest => dest.HwInfo,
                opt => opt.MapFrom(s => s.HwInfo))
            .AfterMap((s, d) => d.NumberOfLicenses = s.Licenses != null && s.Licenses.Any() ? s.Licenses.Count : 0);
        
        CreateMap<Device, DevicePreviewDto>()
            .AfterMap((s, d) => d.NumberOfLicenses = s.Licenses != null && s.Licenses.Any() ? s.Licenses.Count : 0);
        CreateMap<DeviceValidateDto, Device>()
            .ForMember(dest => dest.HwInfo, opt => opt.MapFrom(s => s.HwInfo))
            .ForMember(dest => dest.HWInfoHash, opt => opt.Ignore())
            //.ForMember()
            .ForMember(dest => dest.Activated, opt => opt.MapFrom(s => s.Validity))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(s => s.Customer));
        
        CreateMap<Device, DeviceValidateDto>()
            .ForMember(dest => dest.HwInfo, opt => opt.MapFrom(s => s.HwInfo));
        CreateMap<QueryResult<Device>, QueryResult<DeviceDto>>();
        CreateMap<Device, DeviceDetailDto>().ReverseMap();
        CreateMap<Device, DeviceNameDto>().ReverseMap();
        CreateMap<Device, DeviceNameAndCustomerDto>().ReverseMap();
        
        //Violations
        CreateMap<Violation, ViolationDto>().ReverseMap();
        CreateMap<QueryResult<Violation>, QueryResultDto<ViolationDto>>().ReverseMap();
        
    }
}