using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentScheduler;
namespace SecondHandStoreApp.TaskScheduler
{
    public class MyRegistry : Registry
    {
        public MyRegistry()
        {          
            Schedule<MailingJob>().WithName("TestWriteTask").ToRunNow().AndEvery(10).Seconds();

            // Schedule an IJob to run at an interval
            //Schedule<MyJob>().ToRunNow().AndEvery(2).Seconds();

            // Schedule an IJob to run once, delayed by a specific time interval
            //Schedule<MyJob>().ToRunOnceIn(5).Seconds();

            // Schedule a simple job to run at a specific time
            //Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);

            // Schedule a more complex action to run immediately and on an monthly interval
            // Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

            // Schedule multiple jobs to be run in a single schedule
            // Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
        }
    }
}