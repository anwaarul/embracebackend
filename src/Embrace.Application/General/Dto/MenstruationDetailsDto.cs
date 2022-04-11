using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    [AutoMapTo(typeof(MenstruationDetailsInfo)), AutoMapFrom(typeof(MenstruationDetailsInfo))]

    public class MenstruationDetailsDto : EntityDto<long>
    {
       
        public string UniqueKey { get; set; }

    }
}
