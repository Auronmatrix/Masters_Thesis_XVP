﻿namespace XVP.Domain.Commands.Commands
{
    using Newtonsoft.Json;

    using XVP.Infrastructure.Command.Interfaces;
    using XVP.Infrastructure.Command.Services;
    using XVP.Infrastructure.Shared.Abstracts;

    public class InsertGenericObjectCommand<T> : ICommand
        where T : SerializableResource
    {
        public InsertGenericObjectCommand(string tenant, T item)
        {
            this.Item = item;
            this.Tenant = tenant;
        }

        [JsonProperty]
        public string Tenant { get; private set; }

        [JsonProperty]
        public T Item { get; set; }

        public void Execute()
        {
            IDocumentCommandService<T> service = new DocumentCommandService<T>(this.Tenant);
            var result = service.InsertAsync(this.Item).Result;
        }
    }
}
