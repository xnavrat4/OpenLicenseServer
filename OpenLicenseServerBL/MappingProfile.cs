using AutoMapper;
using Infrastructure.EFCore.Query;
using Mysqlx.Crud;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<User, UserDtoBase>();
       CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();

        CreateMap<UserDto, UserWithTokenDto>();
    }
}