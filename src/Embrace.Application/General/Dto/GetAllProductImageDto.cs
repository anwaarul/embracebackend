using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    public class GetAllProductImageDto
    {
        public long ProductId { get; set; }
        public string ImageUrl { get; set; }
     

    }
}
