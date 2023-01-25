using AutoMapper;
using Infrastructure.EFCore.Query;
//using Mysqlx.Crud;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.DTOs.License;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LicenseDto, License>().ReverseMap();
        
        CreateMap<User, UserDtoBase>();
        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();

        CreateMap<UserDto, UserWithTokenDto>();
    }
}