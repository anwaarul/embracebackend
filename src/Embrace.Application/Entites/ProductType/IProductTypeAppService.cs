using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.ProductType.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductType
{
    public interface IProductTypeAppService : IAsyncCrudAppService<ProductTypeDto, long, PagedResultRequestDto, ProductTypeDto, ProductTypeDto>
    {

    }
}
