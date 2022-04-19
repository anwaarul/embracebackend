using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.Size.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.Size
{
    public interface ISizeAppService : IAsyncCrudAppService<SizeDto, long, PagedResultRequestDto, SizeDto, SizeDto>
    {

    }
}
