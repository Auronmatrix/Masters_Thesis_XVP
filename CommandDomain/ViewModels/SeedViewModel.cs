namespace XVP.Domain.Models.ViewModels
{
    using System.Collections.Generic;

    using XVP.Infrastructure.Shared.Abstracts;

    public class NameViewModel
    {
        public string First { get; set; }

        public string Last { get; set; }
    }

    public class FriendViewModel
    {
        public int Id { get; set; }

        public string Friendname { get; set; }
    }

    public class SeedViewModel : SerializableResource
    {
        public int Index { get; set; }

        public string Guid { get; set; }

        public bool IsActive { get; set; }

        public string Balance { get; set; }

        public string Picture { get; set; }

        public int Age { get; set; }

        public string EyeColor { get; set; }

        public virtual NameViewModel Name { get; set; }

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

        public virtual List<FriendViewModel> Friends { get; set; }

        public string Greeting { get; set; }

        public string FavoriteFruit { get; set; }
    }
}