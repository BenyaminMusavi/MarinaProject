//using Quartz;
//using Quartz.Impl;

//namespace Marina.UI.Jobs;

////public class JobChainingListener : IJobListener
////{
////    private static bool previousJobStatus = false;
////    public string Name => "JobChainingListener";
////    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default) => Task.CompletedTask;
////    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
////    {
////        if (!previousJobStatus)
////        {            // اگر قبلی false بوده باشد، انجام کارهای لازم
////            await Task.CompletedTask;
////        }
////    }
////    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
////    {        // آپدیت مقدار وضعیت جاب
////        previousJobStatus = jobException == null;
////        return Task.CompletedTask;
////    }
////}

//public class JobChainingListener : IJobListener
//{
//    private readonly JobStatusService jobStatusService;
//    public string Name => "JobChainingListener";
//    public JobChainingListener(JobStatusService jobStatusService)
//    {
//        this.jobStatusService = jobStatusService;
//    }
//    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default) { return Task.CompletedTask; }
//    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
//    {
//        if (!jobStatusService.PreviousJobStatus)
//        {            // اگر وضعیت قبلی false بوده باشد، انجام کارهای لازم
//            await Task.CompletedTask;
//        }
//    }
//    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
//    {        // آپدیت مقدار وضعیت جاب
//      //  jobStatusService.PreviousJobStatus = jobException == null;
//        return Task.CompletedTask;
//    }
//}


//public class JobStatusService
//{
//    private static bool previousJobStatus = false;
//    public bool PreviousJobStatus { get => previousJobStatus; set => previousJobStatus = value; }
//}




////private bool _previousJobHadError = false;



////_previousJobHadError = jobException != null;



////if (context.JobDetail.Key.Name == "Insert Data Not Imported")



////if (_previousJobHadError)
////{                // Skip the second job if the previous job had an error
////    return;
////}
////var scheduler = context.Scheduler;
////var sendEmailJobKey = new JobKey("Send Email For Supervisor");

////// Schedule the second job (Send Email) to run 5 minutes after the completion of the first job
////var triggerBuilder = Quartz.TriggerBuilder.Create()
////    .WithIdentity("TriggerForSendEmail", "SendEmailGroup")
////    .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Minute))
////    // 5 minutes later
////    .Build();

////await scheduler.ScheduleJob(new JobDetailImpl("SendEmailJob", "SendEmailGroup", typeof(SendEmailForSupervisorJob)), triggerBuilder, cancellationToken);




////if (_previousJobHadError)
////{
////    await Task.FromResult(false);
////}

//// ==================================================================================
////public class ErrorCheckingJobListener : IJobListener
////{
////    private bool _previousJobHadError = false;

////    public string Name => "ErrorCheckingJobListener";

////    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken)
////    {
////        return Task.CompletedTask;
////    }

////    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken)
////    {
////        if (_previousJobHadError)
////        {
////            return Task.FromResult(false);
////        }

////        return Task.CompletedTask;
////    }

////    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken)
////    {
////        _previousJobHadError = jobException != null;
////        return Task.CompletedTask;
////    }
////}