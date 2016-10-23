using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web;
using System.IO;

[assembly: OwinStartup(typeof(Hungry.Services.Startup))]

namespace Hungry.Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            string root = HttpContext.Current.Server.MapPath("~/temp/uploads");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
        }
    }
}
