using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class BlogCategoryInfo : EmbraceProjBaseEntity
    {
        public string Name { get; set; }
        public string ColourCode { get; set; }
        public string ImageUrl { get; set; }
    }
}
