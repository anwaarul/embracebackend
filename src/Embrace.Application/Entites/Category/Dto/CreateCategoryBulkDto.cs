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
    public class CreateCategoryBulkDto
    {
        public List<CreateCategoryDto> createBulkCategory { get; set; }
    }
}
