using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    public class GetAllProductParametersDto : EntityDto<long>
    {
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double Price { get; set; }

    }
}
