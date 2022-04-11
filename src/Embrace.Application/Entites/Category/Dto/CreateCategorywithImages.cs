using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.Category.Dto
{
    [AutoMapTo(typeof(CategoryInfo)), AutoMapFrom(typeof(CategoryInfo))]
    public class CreateCategorywithImages : EntityDto<long>
    {
        public string Name { get; set; }
        public List<ImageBaseUrlDto> BulkImage { get; set; }
        public List<SubCategoryBulkDto> BulkSubCategoryId { get; set; }

    }
}

