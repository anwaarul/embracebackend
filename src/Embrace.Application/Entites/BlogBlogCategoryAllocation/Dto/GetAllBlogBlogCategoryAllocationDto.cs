using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.BlogBlogCategoryAllocation.Dto
{
    public class GetAllBlogBlogCategoryAllocationDto : EntityDto<long>
    {
        public long BlogId { get; set; }
        public string BlogName { get; set; }
        public long BlogCategoryId { get; set; }
        public string BlogCategoryName { get; set; }
    }
}
