using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductCategories.Dto
{
    [AutoMapTo(typeof(ProductCategoriesInfo)), AutoMapFrom(typeof(ProductCategoriesInfo))]
    public class ProductCategoriesDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}
