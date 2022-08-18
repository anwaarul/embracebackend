using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    public class GetAllBlogsWithBlogCategoryDto : EntityDto<long>
    {
        public String CategoryName { get; set; }
        public List<GetAllGeneralBlogDto> BlogsData { get; set; }
    }
}
