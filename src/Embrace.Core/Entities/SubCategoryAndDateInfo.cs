using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class SubCategoryAndDateInfo : EmbraceProjBaseEntity
    {
        public long SubCategoryId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string UniqueKey { get; set; }

    }
}
