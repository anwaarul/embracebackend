using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class UsersDetailsInfo : EmbraceProjBaseEntity
    {
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public DateTime StartDatePeriod { get; set; }
        public DateTime EndDatePeriod { get; set; }
    
    }
}
