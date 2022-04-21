using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.UserProductAllocation.Dto
{
    public class CreateUserProductAllocationDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public string ProductId { get; set; }

    }
}
