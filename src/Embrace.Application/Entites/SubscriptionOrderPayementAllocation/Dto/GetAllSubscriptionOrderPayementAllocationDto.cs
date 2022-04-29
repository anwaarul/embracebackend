using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.SubscriptionOrderPayementAllocation.Dto
{
    public class GetAllSubscriptionOrderPayementAllocationDto : EntityDto<long>
    {
        public long OrderPaymentId { get; set; }
        public string OrderPaymentName{ get; set; }
        public long SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
    }
}
