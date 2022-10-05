using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities.SubscriptionOrderPayementAllocation.Dto
{
    public class GetAllSubscriptionOrderPayementAllocationDto : EntityDto<long>
    {
        public long OrderPaymentId { get; set; }
        public string UserUniqueName { get; set; }
        public string ProductName { get; set; }
        public string VarientName { get; set; }
        public string SizeName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string CountryName { get; set; }
        public string ZipPostalCode { get; set; }
        public long SubscriptionId { get; set; }
        public string UniqueKey { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public long SubscriptionTypeId { get; set; }
        public string SubscriptionTypeName { get; set; }
    }
}
