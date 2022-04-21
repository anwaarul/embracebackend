using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entities
{
    public class ProductParametersInfo : EmbraceProjBaseEntity
    {
        public long ProductTypeId { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string ProductImage { get; set; }
        public long Quantity { get; set; }

    }
}
