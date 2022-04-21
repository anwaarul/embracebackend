using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterSizeAllocation.Dto
{
    [AutoMapTo(typeof(ProductParameterSizeAllocationInfo)), AutoMapFrom(typeof(ProductParameterSizeAllocationInfo))]
    public class ProductParameterSizeAllocationDto : EntityDto<long>
    {
        public long ProductParameterId { get; set; }
        public long SizeId { get; set; }
    }
}
