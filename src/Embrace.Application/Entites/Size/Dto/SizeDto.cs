using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.Size.Dto
{
    [AutoMapTo(typeof(SizeInfo)), AutoMapFrom(typeof(SizeInfo))]
    public class SizeDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}
