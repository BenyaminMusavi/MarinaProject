using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Marina.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Marina.UI.Providers.Repositories;

public interface IUserRepository
{
    Task<bool> Delete(int id);
    Task<List<UserDto>> GetAll();

    //CookieUserItem Register(RegisterVm model);  
    Task<bool> Register(RegisterVm model);
    Task<bool> SetStatus(int id);
    CookieUserItem Validate(LoginVm model);
}

public class UserRepository : IUserRepository
{
    private MarinaDbContext _db;
    public UserRepository(MarinaDbContext db)
    {
        _db = db;
    }

    public CookieUserItem Validate(LoginVm model)
    {
        var userRecords = _db.Users.Where(x => x.UserName == model.Username && x.IsActive).Include(x=>x.Distributor).Include(x=>x.Line).Include(x=>x.Province);

        var results = userRecords.AsEnumerable()
        .Where(m => m.PasswordHash == Hasher.GenerateHash(model.Password, m.Salt))
        .Select(m => new CookieUserItem
        {
            DistributorCode = m.Distributor.Code,
            Line = m.Line.Name,
            Province = m.Province.Name,
            CreatedUtc = m.CreateDate,
            UserId = m.Id
        });

        return results.FirstOrDefault();
    }

    //public CookieUserItem Register(RegisterVm model)
    //{
    //    var salt = Hasher.GenerateSalt();
    //    var hashedPassword = Hasher.GenerateHash(model.Password, salt);

    //    var user = FromUserRegistrationModelToUser(model, hashedPassword, salt);

    //    _db.Users.Add(user);
    //    _db.SaveChanges();

    //    return new CookieUserItem
    //    {
    //        UserId = user.Id,
    //        EmailAddress = user.EmailAddress,
    //        UserName = user.UserName,
    //        CreatedUtc = user.CreateDate
    //    };
    //}
    public async Task<bool> Register(RegisterVm model)
    {
        var currentUser = await _db.Users.SingleOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
        if (currentUser is null)
            return false;
        if (!currentUser.IsActive)
            return false;
        //if (currentUser.UserName == model.UserName || currentUser.DistributorName == model.DistributorName)
        //    return false;
        var salt = Hasher.GenerateSalt();
        var hashedPassword = Hasher.GenerateHash(model.Password, salt);

        var user = FromUserRegistrationModelToUser(model, currentUser, hashedPassword, salt);

        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return true;
    }

    private static User FromUserRegistrationModelToUser(RegisterVm userRegistration, User user, string hashedPassword, string salt)
    {
        user.DistributorName = userRegistration.DistributorName;
        user.RegionId = userRegistration.RegionId;
        user.RSMId = userRegistration.RSMId;
        user.UserName = userRegistration.UserName;
        user.DistributorId = userRegistration.DistributorId;
        user.LineId = userRegistration.LineId;
        user.ProvinceId = userRegistration.ProvinceId;
        user.PasswordHash = hashedPassword;
        user.Salt = salt;
        user.UpdateTime = DateTime.Now;
        //return new User
        //{
        //    DistributorName = userRegistration.DistributorName,
        //    RegionId = userRegistration.RegionId,
        //    PasswordHash = hashedPassword,
        //    Salt = salt,
        //    UserName = userRegistration.UserName,
        //    RSMId = userRegistration.RSMId,
        //    LineId = userRegistration.LineId,
        //    ProvinceId = userRegistration.ProvinceId,
        //};
        return user;
    }

    public async Task<List<UserDto>> GetAll()
    {
        return await _db.Users.Select(x => new UserDto
        {
            Id = x.Id,
            DistributorName = x.DistributorName,
            RegionId = x.RegionId,
            RSMId = x.RSMId,
            DistributorId = x.DistributorId,
            ProvinceId = x.ProvinceId,
            IsActive = x.IsActive,
            IsDeleted = x.IsDeleted
        }).Where(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var model = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (model != null)
        {
            model.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> SetStatus(int id)
    {
        var model = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (model is null)
            return false;

        if (model.IsActive)
            model.IsActive = false;
        else
            model.IsActive = true;

        await _db.SaveChangesAsync();
        return true;
    }

}