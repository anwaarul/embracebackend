using Abp.AutoMapper;
using Embrace.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    [AutoMapTo(typeof(OrderPlacementInfo)), AutoMapFrom(typeof(OrderPlacementInfo))]
    public class CreateOrderPlacementBulkDto
    {
        public List<CreateOrderPlacementDto> createBulkOrderPlacement { get; set; }
       
    }
}
