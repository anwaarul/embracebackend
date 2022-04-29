using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.SubscriptionType.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.SubscriptionType
{
    public interface ISubscriptionTypeAppService : IAsyncCrudAppService<SubscriptionTypeDto, long, PagedResultRequestDto, SubscriptionTypeDto, SubscriptionTypeDto>
    {

    }
}
