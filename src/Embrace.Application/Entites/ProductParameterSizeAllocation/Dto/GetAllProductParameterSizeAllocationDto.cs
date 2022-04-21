using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameterSizeAllocation.Dto
{
    public class GetAllProductParameterSizeAllocationDto : EntityDto<long>
    {
        public long ProductParameterId { get; set; }
        public string ProductParameterName { get; set; }
        public long SizeId { get; set; }
        public string SizeName { get; set; }
    }
}
