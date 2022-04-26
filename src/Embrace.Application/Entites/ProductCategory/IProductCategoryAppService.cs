using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.ProductCategory.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductCategory
{
    public interface IProductCategoryAppService : IAsyncCrudAppService<ProductCategoryDto, long, PagedResultRequestDto, ProductCategoryDto, ProductCategoryDto>
    {

    }
}
