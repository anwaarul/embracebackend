using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.SubscriptionOrderPayementAllocation.Dto
{
    [AutoMapTo(typeof(SubscriptionOrderPayementAllocationInfo)), AutoMapFrom(typeof(SubscriptionOrderPayementAllocationInfo))]
    public class SubscriptionOrderPayementAllocationDto : EntityDto<long>
    {
        public long OrderPaymentId { get; set; }
        public long SubscriptionId { get; set; }
    }
}
