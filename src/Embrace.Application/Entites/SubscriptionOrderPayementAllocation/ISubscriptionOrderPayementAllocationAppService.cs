using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.SubscriptionOrderPayementAllocation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.SubscriptionOrderPayementAllocation
{
    public interface ISubscriptionOrderPayementAllocationAppService : IAsyncCrudAppService<SubscriptionOrderPayementAllocationDto, long, PagedResultRequestDto, CreateSubscriptionOrderPayementAllocationDto, SubscriptionOrderPayementAllocationDto>
    {

    }
}
