using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    public class PagedResultRequestExtendedDto : PagedResultRequestDto
    {
        public int TenantId { get; set; }
    }
}
