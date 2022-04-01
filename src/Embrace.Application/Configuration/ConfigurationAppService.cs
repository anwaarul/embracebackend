using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Embrace.Configuration.Dto;

namespace Embrace.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : EmbraceAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
