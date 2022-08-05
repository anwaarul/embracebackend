using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.BlogCategory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.BlogCategory
{
    public interface IBlogCategoryAppService : IAsyncCrudAppService<BlogCategoryDto, long, PagedResultRequestDto, BlogCategoryDto, BlogCategoryDto>
    {

    }
}
