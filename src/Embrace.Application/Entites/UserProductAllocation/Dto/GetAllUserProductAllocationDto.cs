using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.UserProductAllocation.Dto
{
    public class GetAllUserProductAllocationDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UniqueKey { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }

    }
}
