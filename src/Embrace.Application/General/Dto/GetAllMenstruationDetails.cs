using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
  
    public class GetAllMenstruationDetails 
    {
        public string UniqueKey { get; set; }
        public DateTime MyCycle { get; set; }
        public DateTime Ovulation_day { get; set; }
        public DateTime Last_Mens_Start { get; set; }
        public DateTime Last_Mens_End { get; set; }
        public DateTime Next_Mens_Start { get; set; }
        public DateTime Next_Mens_End { get; set; }
        public DateTime Ovulation_Window_Start { get; set; }
        public DateTime Ovulation_Window_End { get; set; }
    }
}
