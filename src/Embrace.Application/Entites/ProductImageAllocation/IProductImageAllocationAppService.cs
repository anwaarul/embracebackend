using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entites.ProductImageAllocation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.ProductImageAllocation
{
    public interface IProductImageAllocationAppService : IAsyncCrudAppService<ProductImageAllocationDto, long, PagedResultRequestDto, ProductImageAllocationDto>
    {
        new Task<PagedResultDto<ProductImageAllocationDto>> GetAllAsync(PagedResultRequestDto input);
    }
}
