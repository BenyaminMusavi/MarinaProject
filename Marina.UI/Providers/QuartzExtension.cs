using Marina.UI.Jobs;
using Quartz;
using Quartz.Impl.Matchers;

namespace Marina.UI.Providers;

public static class QuartzExtension
{
    public static void AddQuartz(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(c =>
        {
            c.UseMicrosoftDependencyInjectionJobFactory();
            c.UseSimpleTypeLoader();
            c.UseInMemoryStore();

            // Define and schedule the job to insert data not imported
            var insertDataJobKey = new JobKey("Insert Data Not Imported");
            c.AddJob<InsertDataNotImportedJob>(j => j.WithIdentity(insertDataJobKey));
            c.AddTrigger(t => t.ForJob(insertDataJobKey)
            .WithIdentity("Insert Data Not Imported Trigger")
            .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(21, 04)));

            //Define and schedule the job to send email for supervisor
           var sendEmailJobKey = new JobKey("Send Email For Supervisor");
           c.AddJob<SendEmailForSupervisorJob>(j => j.WithIdentity(sendEmailJobKey));
            c.AddTrigger(t => t.ForJob(sendEmailJobKey)
            .WithIdentity("Send Email For Supervisor Trigger")
            .StartAt(DateBuilder.FutureDate(2, IntervalUnit.Minute)));


            c.AddJobListener<JobChainingListener>();
            //c.AddJobListener<JobChainingListener>(GroupMatcher<JobKey>.AnyGroup());
        });


        builder.Services.AddQuartzHostedService(c =>
        {
            c.WaitForJobsToComplete = true;
        });


    }

}





// Fire 1 minute after the previous job completion
// Add a listener to chain jobs




//builder.Services.AddQuartz(c =>
//{
//    c.UseMicrosoftDependencyInjectionJobFactory();

//    c.UseSimpleTypeLoader();
//    c.UseInMemoryStore();

//    // Define and schedule the job to insert data not imported
//    var insertDataJobKey = new JobKey("Insert Data Not Imported");
//    c.AddJob<InsertDataNotImportedJob>(j => j.WithIdentity(insertDataJobKey));
//    c.AddTrigger(t => t.ForJob(insertDataJobKey)
//        .WithIdentity("Insert Data Not Imported Trigger")
//        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(10, 11)));

//    // Define and schedule the job to send email for supervisor
//    var sendEmailJobKey = new JobKey("Send Email For Supervisor");
//    c.AddJob<SendEmailForSupervisorJob>(j => j.WithIdentity(sendEmailJobKey));
//    c.AddTrigger(t => t.ForJob(sendEmailJobKey)
//       .WithIdentity("Send Email For Supervisor Trigger")
//       .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Minute))); // Fire 5 minutes after the previous job


//});
//var schedulerFactory = new StdSchedulerFactory();
//var scheduler = await schedulerFactory.GetScheduler();

//scheduler.ListenerManager.AddJobListener(new ErrorCheckingJobListener());



// Define and schedule the job to update column has imported
//var updateColumnJobKey = new JobKey("Update Column Has Imported");
//c.AddJob<UpdateColumnHasImportedJob>(j => j.WithIdentity(updateColumnJobKey));
//c.AddTrigger(t => t.ForJob(updateColumnJobKey)
//   .WithIdentity("Update Column Has Imported Trigger")
//   .StartAt(DateBuilder.FutureDate(2, IntervalUnit.Minute))); // Fire 10 minutes after the previous job

//c.AddDependency(sendEmailJobKey, insertDataJobKey);
//Install - Package Quartz.Extensions.DependencyInjection

//await scheduler.Start();


//var sendEmailJobKey = new JobKey("Send Email For Supervisor");
//c.AddJob<SendEmailForSupervisorJob>(j => j.WithIdentity(sendEmailJobKey));
//            c.AddTrigger(t => t.ForJob(sendEmailJobKey)
//            .WithIdentity("Send Email For Supervisor Trigger")
//            .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Minute)) // Fire 5 minutes after the previous job
//            .WithMisfireHandlingInstructionDoNothing());


