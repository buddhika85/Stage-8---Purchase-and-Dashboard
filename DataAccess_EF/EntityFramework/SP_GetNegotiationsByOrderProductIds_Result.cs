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
    
    public partial class SP_GetNegotiationsByOrderProductIds_Result
    {
        public int id { get; set; }
        public int productId { get; set; }
        public Nullable<decimal> quantity { get; set; }
        public Nullable<decimal> negotiatedPricePerItem { get; set; }
        public Nullable<decimal> totalAmount { get; set; }
        public string status { get; set; }
        public System.DateTime negotiationDateTime { get; set; }
        public Nullable<int> orderId { get; set; }
    }
}