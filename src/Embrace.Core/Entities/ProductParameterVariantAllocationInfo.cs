using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class ProductParameterVariantAllocationInfo : EmbraceProjBaseEntity
    {
        public long ProductParameterId { get; set; }
        public long ProductVariantId { get; set; }
    }
}
