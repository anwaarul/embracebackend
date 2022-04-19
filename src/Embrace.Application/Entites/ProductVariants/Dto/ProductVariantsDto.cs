using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductVariants.Dto
{
    [AutoMapTo(typeof(ProductVariantsInfo)), AutoMapFrom(typeof(ProductVariantsInfo))]
    public class ProductVariantsDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}
