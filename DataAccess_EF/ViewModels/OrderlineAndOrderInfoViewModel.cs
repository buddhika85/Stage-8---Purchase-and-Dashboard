using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_EF.ViewModels
{
    public class OrderlineAndOrderInfoViewModel
    {
        public int orderlineId { get; set; }
        public int productListId { get; set; }
        public string model { get; set; }
        public int availableStock { get; set; }
        public decimal quantity { get; set; }
        public decimal negotiatedPricePerItem { get; set; }
        public decimal marketValueInGBP { get; set; }
        public decimal marketValueInSpecificCurrencyForToday { get; set; }
        public decimal total { get; set; }
        public string status { get; set; }
        public DateTime orderlineDateTime { get; set; }
        public string orderlineDate { get; set; }
        public string orderlineTime { get; set; }
        public int orderId { get; set; }
        public string currency { get; set; }
        public int contactId { get; set; }
        public string contactFulName { get; set; }
        public int customerId { get; set; }
        public string customer { get; set; }
    }
}
