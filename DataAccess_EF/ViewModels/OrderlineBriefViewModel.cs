using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_EF.ViewModels
{
    // a less detailed view model for internal apps to explain the orderline details
    public class OrderlineBriefViewModel
    {
        public int id { get; set; }
        public int productId { get; set; }
        public string model { get; set; }
        public Nullable<decimal> quantity { get; set; }
        public Nullable<decimal> negotiatedPricePerItem { get; set; }
        public Nullable<decimal> totalAmount { get; set; }
        public string status { get; set; }
        public Nullable<int> orderId { get; set; }   
    }
}
