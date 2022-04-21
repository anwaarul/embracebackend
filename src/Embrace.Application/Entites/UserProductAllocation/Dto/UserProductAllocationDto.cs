using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.UserProductAllocation.Dto
{
    [AutoMapTo(typeof(UserProductAllocationInfo)), AutoMapFrom(typeof(UserProductAllocationInfo))]
    public class UserProductAllocationDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
    }
}
