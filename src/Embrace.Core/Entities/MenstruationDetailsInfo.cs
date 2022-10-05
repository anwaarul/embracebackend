﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class MenstruationDetailsInfo : EmbraceProjBaseEntity
    {
        public string UniqueKey { get; set; }
        public DateTime MyCycle { get; set; }
        public DateTime Ovulation_date { get; set; }
        public DateTime Last_Mens_Start { get; set; }
        public DateTime Last_Mens_End { get; set; }
        public DateTime Next_Mens_Start { get; set; }
        public DateTime Next_Mens_End { get; set; }
        public DateTime Ovulation_Window_Start { get; set; }
        public DateTime Ovulation_Window_End { get; set; }

    }
}
