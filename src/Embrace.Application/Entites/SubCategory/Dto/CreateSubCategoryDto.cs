using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.SubCategory.Dto
{
    [AutoMapTo(typeof(SubCategoryInfo))]
    public class CreateSubCategoryDto : EntityDto<long>
    {
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
