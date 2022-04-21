using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.ProductParameterSizeAllocation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterSizeAllocation
{
    public interface IProductParameterSizeAllocationAppService : IAsyncCrudAppService<ProductParameterSizeAllocationDto, long, PagedResultRequestDto, CreateProductParameterSizeAllocationDto, ProductParameterSizeAllocationDto>
    {

    }
}
