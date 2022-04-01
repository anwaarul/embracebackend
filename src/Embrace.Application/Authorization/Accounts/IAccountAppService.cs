using System.Threading.Tasks;
using Abp.Application.Services;
using Embrace.Authorization.Accounts.Dto;

namespace Embrace.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
