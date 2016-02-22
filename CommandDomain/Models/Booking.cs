using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XVP.Domain.Models.ViewModels
{
    using Microsoft.Azure.Documents;

    using XVP.Domain.Models.Models;

    public class Booking
    {
        public List<Campaign> Campaigns { get; set; }

        public double Total { get; set; }

        public int Status { get; set; }

        public Guid UserGuid { get; set; }
    }
}
