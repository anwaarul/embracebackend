using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.BlogBlogCategoryAllocation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.BlogBlogCategoryAllocation
{
    public interface IBlogBlogCategoryAllocationAppService : IAsyncCrudAppService<BlogBlogCategoryAllocationDto, long, PagedResultRequestDto, CreateBlogBlogCategoryAllocationDto, BlogBlogCategoryAllocationDto>
    {

    }
}
