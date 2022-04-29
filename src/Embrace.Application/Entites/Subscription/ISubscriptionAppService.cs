using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.Subscription.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.Subscription
{
    public interface ISubscriptionAppService : IAsyncCrudAppService<SubscriptionDto, long, PagedResultRequestDto, SubscriptionDto, SubscriptionDto>
    {

    }
}
