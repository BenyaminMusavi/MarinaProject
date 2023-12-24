using Microsoft.EntityFrameworkCore;
using Marina.UI.Infrastructure;
using Marina.UI.Models.Entities;
using Quartz;

namespace Marina.UI.Jobs;
public class SendEmailForSupervisorsJob : IJob
{
    private MarinaDbContext _db;

    public SendEmailForSupervisorsJob(MarinaDbContext db)
    {
        _db = db;
    }

    public Task Execute(IJobExecutionContext context)
    {
        var supervisor = _db.Supervisors.Include(s => s.Users)
            .FirstOrDefault(s => s.Id == 1);

        var users = supervisor.Users.Where(x => !x.HasImported).ToList();

        string emailBody = "";
        foreach (var user in users)
        {
            emailBody += $"{user.DName} , ";
        }

        INotification notification = new EmailNotification();
        notification.Send(emailBody, supervisor.Email);

        return Task.CompletedTask;
    }
}
