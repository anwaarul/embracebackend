using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.Subscription.Dto
{
    public class GetAllSubscriptionDto : EntityDto<long>
    {
        public string UniqueKey { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public long SubscriptionTypeId { get; set; }
        public string SubscriptionTypeName { get; set; }
    }
}
