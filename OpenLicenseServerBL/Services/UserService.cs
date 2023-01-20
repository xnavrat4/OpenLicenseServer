using AutoMapper;
using OpenLicenseServerBL.QueryObjects;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;
using Infrastructure.Repository;

namespace OpenLicenseServerBL.Services;

public class UserService : BaseService<User>
{
    private readonly UserQueryObject _userQueryObject;
    public UserService(IRepository<User> repository) : base(repository)
    {
    }
    
}