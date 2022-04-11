using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entites.SubCategory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.SubCategory
{
    public interface ISubCategoryAppService : IAsyncCrudAppService<SubCategoryDto, long, PagedResultRequestDto, SubCategoryDto, SubCategoryDto>
    {


    }
}
