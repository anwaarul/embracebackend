using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductCategory.Dto
{
    public class PagedProductCategoryResultRequestExtendedDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
