using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.SubscriptionOrderPayementAllocation.Dto
{
    public class CreateSubscriptionOrderPayementAllocationDto : EntityDto<long>
    {
        public string OrderPaymentId { get; set; }
        public long SubscriptionId { get; set; }

    }
}
