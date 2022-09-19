using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.General.Dto
{
    public class GetAllOrderPlacementDto
    {
        public string UserUniqueName { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

     
    }
}
