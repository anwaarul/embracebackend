using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Embrace.Authorization;

namespace Embrace
{
    [DependsOn(
        typeof(EmbraceCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class EmbraceApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<EmbraceAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(EmbraceApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
