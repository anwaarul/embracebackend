using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entities.ProductVariants.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductVariants
{
    public interface IProductVariantsAppService : IAsyncCrudAppService<ProductVariantsDto, long, PagedResultRequestDto, ProductVariantsDto, ProductVariantsDto>
    {

    }
}
