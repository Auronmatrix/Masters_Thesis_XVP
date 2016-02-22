using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XVP.Domain.Models.ViewModels
{
    using XVP.Infrastructure.Shared.Abstracts;

    public class Motif : SerializableResource
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }
}
