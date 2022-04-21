using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterSizeAllocation.Dto
{
    public class CreateProductParameterSizeAllocationDto : EntityDto<long>
    {
        public long ProductParameterId { get; set; }
        public string SizeId { get; set; }

    }
}
