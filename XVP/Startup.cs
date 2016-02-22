using Microsoft.Owin;

using XVP.Presentation.MVC;

[assembly: OwinStartup(typeof(Startup))]

namespace XVP.Presentation.MVC
{
    using System.Collections.Generic;

    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }

    internal class Person
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public Person Child { get; set; }

        public List<string> NickNames { get; set; }
    }
}
