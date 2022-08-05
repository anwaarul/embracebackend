using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.Blog.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.Blog
{
    public interface IBlogAppService : IAsyncCrudAppService<GetAllBlogDto, long, PagedResultRequestDto, GetAllBlogDto, GetAllBlogDto>
    {

    }
}
