using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductCategory.Dto
{
    [AutoMapTo(typeof(ProductCategoryInfo)), AutoMapFrom(typeof(ProductCategoryInfo))]
    public class ProductCategoryDto : EntityDto<long>
    {
        public string Name { get; set; }
       

    }
}
