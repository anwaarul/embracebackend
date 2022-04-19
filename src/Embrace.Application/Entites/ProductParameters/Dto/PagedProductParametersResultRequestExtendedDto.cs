using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.ProductParameters.Dto
{
    public class PagedProductParametersResultRequestExtendedDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
