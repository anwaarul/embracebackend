using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.ProductParameters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameters
{
    public interface IProductParametersAppService : IAsyncCrudAppService<ProductParametersDto, long, PagedResultRequestDto, ProductParametersDto, ProductParametersDto>
    {

    }
}
