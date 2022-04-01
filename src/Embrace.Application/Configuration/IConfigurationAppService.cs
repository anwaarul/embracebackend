using System.Threading.Tasks;
using Embrace.Configuration.Dto;

namespace Embrace.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
