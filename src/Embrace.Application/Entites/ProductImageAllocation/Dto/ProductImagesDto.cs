using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.ProductImageAllocation.Dto
{
   
    public class ProductImagesDto : EntityDto<long>
    {

        public string BaseUrl { get; set; }

    }
}
