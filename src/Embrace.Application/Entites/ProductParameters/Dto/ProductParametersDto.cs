using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameters.Dto
{
    [AutoMapTo(typeof(ProductParametersInfo)), AutoMapFrom(typeof(ProductParametersInfo))]
    public class ProductParametersDto : EntityDto<long>
    {
        public long ProductCategoryId { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string ProductImage { get; set; }
        public long Quantity { get; set; }

    }
}
