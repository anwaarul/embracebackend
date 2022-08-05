using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class BlogBlogCategoryAllocationInfo : EmbraceProjBaseEntity
    {
        public long BlogId { get; set; }
        public long BlogCategoryId { get; set; }
    }
}
