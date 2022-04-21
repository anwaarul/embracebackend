using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterVariantAllocation.Dto
{
    [AutoMapTo(typeof(ProductParameterVariantAllocationInfo)), AutoMapFrom(typeof(ProductParameterVariantAllocationInfo))]
    public class ProductParameterVariantAllocationDto : EntityDto<long>
    {
        public long ProductParameterId { get; set; }
        public long ProductVariantId { get; set; }
    }
}
