using AutoMapper;
using Infrastructure.EFCore.Query;
using OpenLicenseServerBL.QueryObjects;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;
using Infrastructure.Repository;

namespace OpenLicenseServerBL.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly UserQueryObject _userQueryObject;
    public UserService(IRepository<User> userRepository, UserQueryObject userQueryObject)
    {
        _userRepository = userRepository;
        _userQueryObject = userQueryObject;
    }

    public async Task CreateAsync(User user)
    {
        await _userRepository.Insert(user);
    }

    public async Task<User?> GetById(int id)
    {
        return await _userRepository.GetById(id);
    }

    public async Task<QueryResult<User>> GetUserByEmailAsync(string email)
    {
        var user = await Task.Run(() =>
            _userQueryObject.ExecuteQuery(email));
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _userRepository.Update(user);
        await _userRepository.Save();
    }

    public async Task DeleteAsync(int id)
    {
        await _userRepository.Delete(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.Get();
    }
}