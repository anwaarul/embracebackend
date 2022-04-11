using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class SubCategoryImageAllocationInfo : EmbraceProjBaseEntity
    {
        public long SubCategoryId { get; set; }
        public string ImageUrl { get; set; }
    
    }
}
