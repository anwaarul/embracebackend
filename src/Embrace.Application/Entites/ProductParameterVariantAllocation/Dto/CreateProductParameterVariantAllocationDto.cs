using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterVariantAllocation.Dto
{
    public class CreateProductParameterVariantAllocationDto : EntityDto<long>
    {
        public long ProductParameterId { get; set; }
        public string ProductVariantId { get; set; }

    }
}
