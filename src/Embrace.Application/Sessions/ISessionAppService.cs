using System.Threading.Tasks;
using Abp.Application.Services;
using Embrace.Sessions.Dto;

namespace Embrace.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
