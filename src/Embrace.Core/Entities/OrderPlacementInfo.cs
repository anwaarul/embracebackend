using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class OrderPlacementInfo : EmbraceProjBaseEntity
    {
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
    }
}
