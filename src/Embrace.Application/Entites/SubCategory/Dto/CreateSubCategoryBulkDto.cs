using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Embrace.Entites.SubCategory.Dto;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.SubCategory.Dto
{
    public class CreateSubCategoryBulkDto
    {
        public List<CreateSubCategoryDto> createBulkSubCategory { get; set; }
    }
}
