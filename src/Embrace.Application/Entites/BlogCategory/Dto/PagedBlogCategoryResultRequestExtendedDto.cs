using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.BlogCategory.Dto
{
    public class PagedBlogCategoryResultRequestExtendedDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
