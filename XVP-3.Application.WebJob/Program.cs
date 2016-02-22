namespace XVP.Application.WebJob
{
    using System;

    using Microsoft.Azure.WebJobs;

    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        static void Main()
        {
            var config = new JobHostConfiguration();
            config.Queues.MaxPollingInterval = TimeSpan.FromMinutes(0.05);    
            var host = new JobHost();
            host.RunAndBlock();
        }
    }
}
