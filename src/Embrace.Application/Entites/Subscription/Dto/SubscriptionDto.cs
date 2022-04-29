using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.Subscription.Dto
{
    [AutoMapTo(typeof(SubscriptionInfo)), AutoMapFrom(typeof(SubscriptionInfo))]
    public class SubscriptionDto : EntityDto<long>
    {
        public string UniqueKey { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public long SubscriptionTypeId { get; set; }
    }
}
