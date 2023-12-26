//using Marina.UI.Models.Entities;
//using Quartz;

//namespace Marina.UI.Jobs;

//public class UpdateColumnHasImportedJob : IJob
//{
//    private readonly MarinaDbContext _db;

//    public UpdateColumnHasImportedJob(MarinaDbContext db)
//    {
//        _db = db;
//    }

//    public async Task Execute(IJobExecutionContext context)
//    {
//        //var notImportedDatas = _db.NotImportedDatas.Include(c => c.Supervisor);
//        //var a = notImportedDatas.OrderByDescending(c => c.Id).LastOrDefault(c => c.PersonName == "Done");

//        //if (a is not null)
//        //{
//        foreach (var data in _db.Users)
//        {
//            data.HasImported = false;
//        }

//        await _db.SaveChangesAsync();

//        //}

//        await Task.CompletedTask;

//    }
//}