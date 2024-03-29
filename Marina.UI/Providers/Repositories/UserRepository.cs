﻿using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Marina.UI.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Marina.UI.Providers.Repositories;

public interface IUserRepository
{
    Task<bool> Delete(int id);
    Task<List<UserDto>> GetAll();
    Task HasImported(int id);

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
        var userRecords = _db.Users.Where(x => x.UserName == model.Username && x.IsActive).Include(x => x.Distributor).Include(x => x.Line).Include(x => x.Province);

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
        //var currentUser = await _db.Users.SingleOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
        //if (currentUser is null)
        //    return false;
        //if (!currentUser.IsActive)
        //    return false;
        var userNameTable = CalculateUserNameTable(model.DistributorId, model.ProvinceId, model.LineId);
        var hasTable = await Helper.TableExists(userNameTable);
        if (hasTable)
            return false;

        var userRecords = await _db.Users.Where(x => x.UserName == model.UserName.Trim()).FirstOrDefaultAsync();

        if (userRecords is not null)
            return false;

        var salt = Hasher.GenerateSalt();
        var hashedPassword = Hasher.GenerateHash(model.Password, salt);

        var user = FromUserRegistrationModelToUser(model, hashedPassword, salt);

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return true;
    }

    private static User FromUserRegistrationModelToUser(RegisterVm userRegistration, string hashedPassword, string salt)
    {
        //user.DistributorName = userRegistration.DistributorName;
        //user.RegionId = userRegistration.RegionId;
        //user.RSMId = userRegistration.RSMId;
        //user.UserName = userRegistration.UserName;
        //user.DistributorId = userRegistration.DistributorId;
        //user.LineId = userRegistration.LineId;
        //user.ProvinceId = userRegistration.ProvinceId;
        //user.PasswordHash = hashedPassword;
        //user.Salt = salt;
        //user.UpdateTime = DateTime.Now;
        return new User
        {
            DName = userRegistration.DistributorName,
            RegionId = userRegistration.RegionId,
            PasswordHash = hashedPassword,
            Salt = salt,
            UserName = userRegistration.UserName.Trim(),
            RSMId = userRegistration.RSMId,
            LineId = userRegistration.LineId,
            ProvinceId = userRegistration.ProvinceId,
            DistributorId = userRegistration.DistributorId,
            PhoneNumber = userRegistration.PhoneNumber,
            SupervisorId = userRegistration.SupervisorId,
            NsmId = userRegistration.NsmId
        };
        //return user;
    }

    public async Task<List<UserDto>> GetAll()
    {
        return await _db.Users.Select(x => new UserDto
        {
            Id = x.Id,
            DistributorName = x.DName,
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

    public async Task HasImported(int id)
    {
        var model = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        model.HasImported = true;
        await _db.SaveChangesAsync();
    }

    private string CalculateUserNameTable(int distributorCode, int provinceId, int lineId)
    {
        var distributor = _db.Distributors.FirstOrDefault(x => x.Id == distributorCode);
        var distributorName = distributor?.Code;
        var province = _db.Provinces.FirstOrDefault(x => x.Id == provinceId);
        var provinceName = province?.Name;
        var line = _db.Lines.FirstOrDefault(x => x.Id == lineId);
        var lineName = line?.Name;
        var userNameTable = $"{distributorName}_{provinceName}_{lineName}";
        return userNameTable;
    }

}