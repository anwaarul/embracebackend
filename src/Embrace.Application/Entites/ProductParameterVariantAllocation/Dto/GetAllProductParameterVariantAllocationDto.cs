using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterVariantAllocation.Dto
{
    public class GetAllProductParameterVariantAllocationDto : EntityDto<long>
    {
        public long ProductParameterId { get; set; }
        public string ProductParameterName { get; set; }
        public long ProductVariantId { get; set; }
        public string ProductVariantName { get; set; }
    }
}
