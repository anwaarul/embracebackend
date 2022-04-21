using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class UserProductAllocationInfo : EmbraceProjBaseEntity
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
    }
}
