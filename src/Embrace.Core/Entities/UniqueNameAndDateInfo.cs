using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class UniqueNameAndDateInfo : EmbraceProjBaseEntity
    {
        public string Name { get; set; }
        public DateTime DateAndTime { get; set; }
        public string UniqueKey { get; set; }
    }
}
