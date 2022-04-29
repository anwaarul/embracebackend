using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class SubscriptionOrderPayementAllocationInfo : EmbraceProjBaseEntity
    {
        public long OrderPaymentId { get; set; }
        public long SubscriptionId { get; set; }
    }
}
