using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.ProductCategories.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductCategories
{
    public interface IProductCategoriesAppService : IAsyncCrudAppService<ProductCategoriesDto, long, PagedResultRequestDto, ProductCategoriesDto, ProductCategoriesDto>
    {

    }
}
