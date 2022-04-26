using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entites.Category.Dto;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.ProductImageAllocation.Dto
{
    [AutoMapTo(typeof(ProductImageAllocationInfo)), AutoMapFrom(typeof(ProductImageAllocationInfo))]
    public class CreateProductImagesAllocationDto : EntityDto<long>
    {
        public long ProductParameterId { get; set; }
        public List<ProductImagesDto> BulkImage { get; set; }


    }
}
