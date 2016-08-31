using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace SecondHandStoreApp.TaskScheduler
{
        public class MailingJob : IJob, IRegisteredObject
        {
            private readonly object _lock = new object();

            private bool _shuttingDown;

            public MailingJob()
            {
                // Register this job with the hosting environment.
                // Allows for a more graceful stop of the job, in the case of IIS shutting down.
                HostingEnvironment.RegisterObject(this);
            }

            public void Execute()
            {
                lock (_lock)
                {
                    if (_shuttingDown)
                        return;

                // Do work, son!
                System.Diagnostics.Debug.WriteLine("WORKING");
            }
            }

            public void Stop(bool immediate)
            {
                // Locking here will wait for the lock in Execute to be released until this code can continue.
                lock (_lock)
                {
                    _shuttingDown = true;
                }

                HostingEnvironment.UnregisterObject(this);
            }
        } 
}