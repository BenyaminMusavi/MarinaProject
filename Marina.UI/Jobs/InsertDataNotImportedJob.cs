using Microsoft.EntityFrameworkCore;
using Marina.UI.Models.Entities;
using Quartz;

namespace Marina.UI.Jobs;
public class InsertDataNotImportedJob : IJob
{
    private readonly MarinaDbContext _db;

    public InsertDataNotImportedJob(MarinaDbContext db)
    {
        _db = db;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using (var transaction = await _db.Database.BeginTransactionAsync())
        {
            try
            {
                await Method1();

                await _db.Database.ExecuteSqlRawAsync("UPDATE [User] SET HasImported = 0");

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }
        }

        await Task.CompletedTask;
    }

    private async Task Method1()
    {
        var supervisors = await _db.Supervisors.Include(s => s.Users).ToListAsync();
        var notImportedDataList = supervisors.Select(supervisor => new NotImportedData(
                                                      supervisor.Id,
                                                      string.Join(" , ", supervisor.Users.Where(x => !x.HasImported)
                                                      .Select(u => u.DName))
                                                      )).ToList();
        await _db.NotImportedDatas.AddRangeAsync(notImportedDataList);
        await _db.SaveChangesAsync();

    }
}


// Handle the exception
//throw new JobExecutionException("Job failed", ex, false); // Throw JobExecutionException

// The transaction will be rolled back automatically when the scope is disposed without calling scope.Complete()



//private async Task Method2()
//{
//    var supervisors = await _db.Supervisors.Include(s => s.Users).ToListAsync();
//    var notImportedDataList = new List<NotImportedData>();
//    foreach (var supervisor in supervisors)
//    {
//        var users = supervisor.Users.Where(x => !x.HasImported).Select(u => u.DName).ToList();
//        string userNames = string.Join(" , ", users);

//        var notImport = new NotImportedData(supervisor.Id, userNames);
//        notImportedDataList.Add(notImport);
//    }

//    await _db.NotImportedDatas.AddRangeAsync(notImportedDataList);
//    await _db.SaveChangesAsync();
//}



//var supervisors = _db.Supervisors;

//foreach (var item in supervisors)
//{
//    var supervisor = supervisors.Include(s => s.Users)
//    .FirstOrDefault(s => s.Id == item.Id);

//    var users = supervisor.Users.Where(x => !x.HasImported).ToList();

//    string emailBody = "";
//    foreach (var user in users)
//    {
//        emailBody += $"{user.DName} , ";
//    }

//    INotification notification = new EmailNotification();
//    notification.Send(emailBody, supervisor.Email);
//}

//=================================

//        var users = _db.Supervisors
//    .SelectMany(s => s.Users)
//    .Where(u => !u.HasImported)
//    .Select(u => u.DName);

//string emailBody = string.Join(" , ", users);

//INotification notification = new EmailNotification();
//notification.Send(emailBody, "supervisor1@example.com, supervisor2@example.com");