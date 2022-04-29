using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.OrderPlacement.Dto
{
    public class PagedOrderPlacementResultRequestExtendedDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
