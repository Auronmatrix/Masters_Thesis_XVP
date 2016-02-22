namespace XVP.Domain.Models.Models
{
    using System.Collections.Generic;

    using XVP.Infrastructure.Shared.Abstracts;
    using XVP.Infrastructure.Shared.CustomAttributes;

   [Document("DemoDB", "DemoCollection", false)]
   public class Name
    {
        public string First { get; set; }

        public string Last { get; set; }
    }

   [Document("DemoDB", "DemoCollection", false)]
   public class Friend
    {
        public int Id { get; set; }

        public string Friendname { get; set; }
    }

   [Document("DemoDB", "DemoCollection", false)]
   public class SeedModel : SerializableResource
    {
        public int Index { get; set; }

        public string Guid { get; set; }

        public bool IsActive { get; set; }

        public string Balance { get; set; }

        public string Picture { get; set; }

        public int Age { get; set; }

        public string EyeColor { get; set; }

        public virtual Name Name { get; set; }

        public string Company { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string About { get; set; }

        public string Registered { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public List<string> Tags { get; set; }

        public List<int> Range { get; set; }

        public virtual List<Friend> Friends { get; set; }

        public string Greeting { get; set; }

        public string FavoriteFruit { get; set; }
    }
}