//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess_EF.EntityFramework
{
    using System;
    
    public partial class SP_GetOrderlineAndOrderDetails_Result
    {
        public int orderlineId { get; set; }
        public int productListId { get; set; }
        public string model { get; set; }
        public Nullable<decimal> quantity { get; set; }
        public Nullable<decimal> negotiatedPricePerItem { get; set; }
        public decimal marketValueInGBP { get; set; }
        public Nullable<decimal> marketValueInSpecificCurrencyForToday { get; set; }
        public Nullable<decimal> total { get; set; }
        public string status { get; set; }
        public System.DateTime orderlineDateTime { get; set; }
        public string orderlineDate { get; set; }
        public string orderlineTime { get; set; }
        public Nullable<int> orderId { get; set; }
        public string currency { get; set; }
        public Nullable<int> contactId { get; set; }
        public string contactFulName { get; set; }
        public Nullable<int> customerId { get; set; }
        public string customer { get; set; }
    }
}
