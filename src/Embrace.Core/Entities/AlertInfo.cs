using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class AlertInfo : EmbraceProjBaseEntity
    {
        public string UniqueKey { get; set; }
        public string Message { get; set; }
    }
}
