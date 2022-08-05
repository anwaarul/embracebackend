using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.BlogCategory.Dto
{
    [AutoMapTo(typeof(BlogCategoryInfo)), AutoMapFrom(typeof(BlogCategoryInfo))]
    public class BlogCategoryDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string ColourCode { get; set; }
        public string ImageUrl { get; set; }
    }
}
