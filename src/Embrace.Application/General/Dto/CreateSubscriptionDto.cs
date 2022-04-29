using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    
    public class CreateSubscriptionDto 
    {
        public string UniqueKey { get; set; }
        public string SubscriptionName { get; set; }
        public List<CreateOrderPlacementDto> orderlist { get; set; }
       
    }
}
