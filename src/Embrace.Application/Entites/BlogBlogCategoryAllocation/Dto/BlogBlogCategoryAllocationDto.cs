using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.BlogBlogCategoryAllocation.Dto
{
    [AutoMapTo(typeof(BlogBlogCategoryAllocationInfo)), AutoMapFrom(typeof(BlogBlogCategoryAllocationInfo))]
    public class BlogBlogCategoryAllocationDto : EntityDto<long>
    {
        public long BlogId { get; set; }
        public long BlogCategoryId { get; set; }
    }
}
