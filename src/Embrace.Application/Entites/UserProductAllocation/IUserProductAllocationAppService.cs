using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.UserProductAllocation.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.UserProductAllocation
{
    public interface IUserProductAllocationAppService : IAsyncCrudAppService<UserProductAllocationDto, long, PagedResultRequestDto, CreateUserProductAllocationDto, UserProductAllocationDto>
    {

    }
}
