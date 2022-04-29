using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.OrderPlacement.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.OrderPlacement
{
    public interface IOrderPlacementAppService : IAsyncCrudAppService<OrderPlacementDto, long, PagedResultRequestDto, OrderPlacementDto, OrderPlacementDto>
    {

    }
}
