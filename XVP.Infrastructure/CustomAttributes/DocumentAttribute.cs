namespace XVP.Infrastructure.Shared.CustomAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class DocumentAttribute : Attribute
    {
        public DocumentAttribute(string databaseId, string collectionId, bool tenantSpecific = true)
        {
            this.DatabaseId = databaseId;
            this.CollectionId = collectionId;
            this.TenantSpecific = tenantSpecific;
        }

        public string DatabaseId { get; private set; }

        public string CollectionId { get; private set; }

        public bool TenantSpecific { get; private set; }
    }
}