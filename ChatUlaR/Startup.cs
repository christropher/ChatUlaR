using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatUlaR.Startup))]
namespace ChatUlaR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
