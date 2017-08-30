using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bank.Startup))]
namespace Bank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
