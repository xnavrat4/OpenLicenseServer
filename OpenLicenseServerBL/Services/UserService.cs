using AutoMapper;
using OpenLicenseServerBL.DTOs;
//using FoodliveryBL.QueryObjects;
using OpenLicenseServerDAL.Models;
using Infrastructure.Repository;

namespace OpenLicenseServerBL.Services;

public class UserService : BaseService<User>
{
    public UserService(IRepository<User> repository) : base(repository)
    {
    }
}