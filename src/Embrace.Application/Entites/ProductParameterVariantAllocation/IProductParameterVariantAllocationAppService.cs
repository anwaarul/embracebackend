using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.ProductParameterVariantAllocation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterVariantAllocation
{
    public interface IProductParameterVariantAllocationAppService : IAsyncCrudAppService<ProductParameterVariantAllocationDto, long, PagedResultRequestDto, CreateProductParameterVariantAllocationDto, ProductParameterVariantAllocationDto>
    {

    }
}
