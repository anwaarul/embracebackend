using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductType.Dto
{
    [AutoMapTo(typeof(ProductTypeInfo)), AutoMapFrom(typeof(ProductTypeInfo))]
    public class ProductTypeDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}
