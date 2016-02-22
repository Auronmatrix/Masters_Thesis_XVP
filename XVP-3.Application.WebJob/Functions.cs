namespace XVP.Application.WebJob
{
    using System.IO;

    using Microsoft.Azure.WebJobs;

    using Newtonsoft.Json;

    using XVP.Domain.Commands;
    using System;

    public class Functions
    {
        public static void ProcessCommandQueue([QueueTrigger("command-queue")] string command, TextWriter log)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var deserialized = JsonConvert.DeserializeObject<ICommand>(command, settings);
            deserialized.Execute();
            log.WriteLine("Processed command");
            var commandType = deserialized.GetType().Name.ToString();
            Console.WriteLine("TENANT: " + deserialized.Tenant + ", COMMAND: " + commandType + ", RESULT: Complete" );
        }
    }
}
