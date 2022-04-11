using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class SubCategoryInfo : EmbraceProjBaseEntity
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    
    }
}
