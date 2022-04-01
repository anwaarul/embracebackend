using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace
{
    public class EmbraceProjBaseEntity : FullAuditedEntity<long>
    {
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
    }
}
