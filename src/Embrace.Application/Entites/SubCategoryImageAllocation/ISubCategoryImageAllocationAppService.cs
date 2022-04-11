using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entites.SubCategoryImageAllocation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.SubCategoryImageAllocation
{
    public interface ISubCategoryImageAllocationAppService : IAsyncCrudAppService<SubCategoryImageAllocationDto, long, PagedResultRequestDto, SubCategoryImageAllocationDto>
    {
        new Task<PagedResultDto<SubCategoryImageAllocationDto>> GetAllAsync(PagedResultRequestDto input);
    }
}
