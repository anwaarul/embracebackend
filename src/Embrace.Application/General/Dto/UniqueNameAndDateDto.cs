using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    [AutoMapTo(typeof(UniqueNameAndDateInfo)), AutoMapFrom(typeof(UniqueNameAndDateInfo))]
    public class UniqueNameAndDateDto 
    {
        public string Name { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
