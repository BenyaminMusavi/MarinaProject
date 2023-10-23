using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Marina.UI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Marina.UI.Providers.Repositories;

public interface IUserRepository
{
    Task<List<UserDto>> GetAll();

    //CookieUserItem Register(RegisterVm model);  
    bool Register(RegisterVm model);
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
        var userRecords = _db.Users.Where(x => x.UserName == model.Username && x.IsActive);

        var results = userRecords.AsEnumerable()
        .Where(m => m.PasswordHash == Hasher.GenerateHash(model.Password, m.Salt))
        .Select(m => new CookieUserItem
        {
            UserId = m.Id,
            EmailAddress = m.EmailAddress,
            UserName = m.UserName,
            CreatedUtc = m.CreateDate
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
    public bool Register(RegisterVm model)
    {
        var salt = Hasher.GenerateSalt();
        var hashedPassword = Hasher.GenerateHash(model.Password, salt);

        var user = FromUserRegistrationModelToUser(model, hashedPassword, salt);

        _db.Users.Add(user);
        _db.SaveChanges();

        return true;
    }

    private static User FromUserRegistrationModelToUser(RegisterVm userRegistration, string hashedPassword, string salt)
    {
        return new User
        {
            EmailAddress = userRegistration.Email,
            FirstName = userRegistration.FirstName,
            LastName = userRegistration.LastName,
            PasswordHash = hashedPassword,
            Salt = salt,
            UserName = userRegistration.UserName,
            AgencyCode = userRegistration.AgencyCode,
            Line = userRegistration.Line.ToString(),
            Province = userRegistration.Province,
        };
    }

    public async Task<List<UserDto>> GetAll()
    {
        return await _db.Users.Select(x => new UserDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            AgencyCode = x.AgencyCode,
            Line = x.Line,
            Province = x.Province,
            IsActive = x.IsActive,
            IsDeleted = x.IsDeleted
        }).ToListAsync();
    }

}