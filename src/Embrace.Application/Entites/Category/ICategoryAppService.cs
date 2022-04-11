using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entites.Category.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.Category
{
    public interface ICategoryAppService : IAsyncCrudAppService<CategoryDto, long, PagedResultRequestDto, CategoryDto, CategoryDto>
    {


    }
}
