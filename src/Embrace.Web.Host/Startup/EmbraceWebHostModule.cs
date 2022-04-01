using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Embrace.Configuration;

namespace Embrace.Web.Host.Startup
{
    [DependsOn(
       typeof(EmbraceWebCoreModule))]
    public class EmbraceWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public EmbraceWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EmbraceWebHostModule).GetAssembly());
        }
    }
}
