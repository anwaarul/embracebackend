using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Embrace.EntityFrameworkCore;
using Embrace.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Embrace.Web.Tests
{
    [DependsOn(
        typeof(EmbraceWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class EmbraceWebTestModule : AbpModule
    {
        public EmbraceWebTestModule(EmbraceEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EmbraceWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(EmbraceWebMvcModule).Assembly);
        }
    }
}