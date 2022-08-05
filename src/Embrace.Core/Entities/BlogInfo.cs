using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class BlogInfo : EmbraceProjBaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public string ImageUrl { get; set; }
    }
}
