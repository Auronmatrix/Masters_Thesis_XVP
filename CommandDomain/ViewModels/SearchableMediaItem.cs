namespace XVP.Domain.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using AzureSearchClient;

    [Serializable]
    public class SearchableMediaItem : Document
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Publisher { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public GeographyPoint Location { get; set; }

        public string Type { get; set; }

        public bool IsAvailable { get; set; }

        public string[] Tenants { get; set; } 

        public override object this[string fieldName]
        {
            get
            {
                var propertyInfo = this.GetType().GetProperty(fieldName);
                return propertyInfo.GetValue(this, null);
            }

            set
            {
                var propertyInfo = this.GetType().GetProperty(fieldName);
                propertyInfo.SetValue(this, value);
            }
        }

        public override IEnumerable<Field> GetFields()
        {
            return new List<Field>
                       {
                           new Field() { Name = "Id", Type = FieldDataTypes.String, IsKey = true },
                           new Field() { Name = "Name", Type = FieldDataTypes.String, IsSuggestionAutoComplete = true },
                           new Field() { Name = "Publisher", Type = FieldDataTypes.String },
                           new Field() { Name = "Price", Type = FieldDataTypes.Double, IsSearchable = false },
                           new Field() { Name = "Location", Type = FieldDataTypes.GeographyPoint,  IsFilterable = true, IsRetrievable = true, IsSearchable = true, IsFacetable = true },
                           new Field() { Name = "Type", Type = FieldDataTypes.String },
                           new Field() { Name = "Image", Type = FieldDataTypes.String },
                           new Field() { Name = "Description", Type = FieldDataTypes.String },
                           new Field() { Name = "Tenants", Type = FieldDataTypes.StringCollection },
                           new Field() { Name = "IsAvailable", Type = FieldDataTypes.Boolean }
                       };
        }  
    }
}