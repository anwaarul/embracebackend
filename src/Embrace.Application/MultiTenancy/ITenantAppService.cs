using Abp.Application.Services;
using Embrace.MultiTenancy.Dto;

namespace Embrace.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

