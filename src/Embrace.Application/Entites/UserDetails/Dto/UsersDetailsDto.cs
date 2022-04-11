using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.UsersDetails.Dto
{
    [AutoMapTo(typeof(UsersDetailsInfo)), AutoMapFrom(typeof(UsersDetailsInfo))]
    public class UsersDetailsDto : EntityDto<long>
    {
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public DateTime StartDatePeriod { get; set; }
        public DateTime EndDatePeriod { get; set; }

    }
}
