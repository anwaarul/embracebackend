using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.BlogBlogCategoryAllocation.Dto
{
    public class CreateBlogBlogCategoryAllocationDto : EntityDto<long>
    {
        public long BlogId { get; set; }
        public string BlogCategoryId { get; set; }

    }
}
