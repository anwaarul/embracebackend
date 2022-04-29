using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class SubscriptionInfo : EmbraceProjBaseEntity
    {
        public string UniqueKey { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public long SubscriptionTypeId { get; set; }
       
    }
}
