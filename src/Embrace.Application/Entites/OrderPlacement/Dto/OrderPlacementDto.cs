using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.OrderPlacement.Dto
{
    [AutoMapTo(typeof(OrderPlacementInfo)), AutoMapFrom(typeof(OrderPlacementInfo))]
    public class OrderPlacementDto : EntityDto<long>
    {
        public string UserUniqueName { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string CountryName { get; set; }
        public string ZipPostalCode { get; set; }
    }
}
