using Marina.UI.Infrastructure;
using Marina.UI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Marina.UI.Jobs;

public class SendEmailForSupervisorJob : IJob
{
    private readonly MarinaDbContext _db;

    public SendEmailForSupervisorJob(MarinaDbContext db)
    {
        _db = db;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // var previousJobStatus = context.MergedJobDataMap.GetBoolean("previousJobStatus");

        var yesterday = DateTime.Now.AddDays(-1).Date;
        var notImportedDatas = _db.NotImportedDatas
            .Include(c => c.Supervisor)
            .Where(c => c.DateTime.Date == yesterday)
            .ToList();

        foreach (var notImportedData in notImportedDatas)
        {
            INotification notification = new EmailNotification();
            var emailBody = notImportedData.PersonName;
            var email = notImportedData.Supervisor.Email;
            notification.Send(emailBody, email);
        }

        await Task.CompletedTask;

    }
}

