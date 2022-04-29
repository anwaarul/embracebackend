using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.SubscriptionType.Dto
{
    [AutoMapTo(typeof(SubscriptionTypeInfo)), AutoMapFrom(typeof(SubscriptionTypeInfo))]
    public class SubscriptionTypeDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}
