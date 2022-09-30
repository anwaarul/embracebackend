using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class UserBlogInfo : EmbraceProjBaseEntity
    {
        public string UniqueKey { get; set; }
        public long BlogId { get; set; }
        public bool IsSavedBlog { get; set; }

    }
}
