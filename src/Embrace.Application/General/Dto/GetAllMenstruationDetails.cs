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
        public int MyCycle { get; set; }
        public int Ovulation_day { get; set; }
        public DateTime Last_ovulation { get; set; }
        public DateTime Next_mens { get; set; }
        public DateTime Next_ovulation { get; set; }
        public DateTime Ovulation_window1 { get; set; }
        public DateTime Ovulation_window2 { get; set; }
        public DateTime Ovulation_window3 { get; set; }
        public DateTime Ovulation_window4 { get; set; }
        public DateTime Safe_period1 { get; set; }
        public DateTime Safe_period2 { get; set; }
        public DateTime Safe_period3 { get; set; }
        public DateTime Safe_period4 { get; set; }
    }
}
