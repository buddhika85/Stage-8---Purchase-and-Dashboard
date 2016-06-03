using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_EF.ViewModels
{
    public class ProductStockInfoViewModel
    {
        public int ProductListId { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Condition { get; set; }
        public string Brand { get; set; }
    }
}
