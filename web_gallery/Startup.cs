using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(web_gallery.Startup))]
namespace web_gallery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
