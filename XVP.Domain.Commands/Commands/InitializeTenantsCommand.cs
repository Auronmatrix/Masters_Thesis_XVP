using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XVP.Domain.Commands.Commands
{
    using Newtonsoft.Json;

    using XVP.Domain.Models.Models;
    using XVP.Infrastructure.Command.Initializers;

    public class InitializeTenantsCommand : ICommand
    {
        private const string DefaultCollection = "Tenants";

        private const string DefaultDatabase = "CoreDB";


        public InitializeTenantsCommand(string json)
        {
            this.Json = json;
        }

        [JsonProperty]
        public string Tenant { get; private set; }

        [JsonProperty]
        public string Json { get; private set; }

        public void Execute()
        {
          var result = DocumentDbInitializer<Tenant>.Initialize(this.Json, DefaultCollection, DefaultDatabase).Result;
        }
    }
}
