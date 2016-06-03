using DataAccess_EF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BCMY.WebAPI.Util
{
    public class HtmlTableGenerator
    {
        // used to generate the html table for product stock info excel file download
        public StringBuilder GenerateModelStockInfoHtmlTable(IEnumerable<ProductStockInfoViewModel> stockInfoModels)
        {
            StringBuilder sb = null;
            try
            {
                sb = new StringBuilder();
                sb.Append("<table border=`" + "1px" + "`b>");
                sb.Append("<tr>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Product list Id</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Quantity</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Category</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Condition</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Brand</font></b></td>");
                sb.Append("</tr>");
                foreach (ProductStockInfoViewModel modelInfo in stockInfoModels)
                {
                    sb.Append("<tr>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + modelInfo.ProductListId.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + modelInfo.Quantity.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + modelInfo.Category.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + modelInfo.Condition.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + modelInfo.Brand.ToString() + "</font></td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                return sb;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        // used to generate the html table for orderline info excel file download
        public StringBuilder GenerateOrderlineInfoHtmlTable(IEnumerable<OrderLineViewModel> orderlines, OrderViewModel orderInfo)
        {
            StringBuilder sb = null;
            try
            {
                sb = new StringBuilder();
                sb.Append("<tr>Order ID - " + orderInfo.id +"</tr>");
                sb.Append("<tr>Order Status - " + orderInfo.status + "</tr>");
                sb.Append("<tr>Customer - " + orderInfo.company + "</tr>");
                sb.Append("<tr>Contact - " + orderInfo.contactFulName + "</tr>");
                sb.Append("<tr>Order Date - " + orderInfo.orderCreationDate + "</tr>");
                sb.Append("<tr>Order Total - " + CommonBehaviour.GetCurrencySymbol(orderInfo.currency) + " " + orderInfo.total + "</tr>");
                sb.Append("<tr>" + "</br>" + "</tr>");
                sb.Append("<table border=`" + "1px" + "`b>");
                sb.Append("<tr>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Condition</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Brand</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Model</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Quantity</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>PPI</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Total(" + CommonBehaviour.GetCurrencySymbol(orderInfo.currency) + ")</font></b></td>");
                sb.Append("<td><b><font face=Arial Narrow size=3>Status</font></b></td>");
                sb.Append("</tr>");
                foreach (OrderLineViewModel orderline in orderlines)
                {
                    sb.Append("<tr>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + orderline.condition.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + orderline.brand.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + orderline.model.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + orderline.quantity.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + orderline.negotiatedPricePerItem.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + orderline.totalAmount.ToString() + "</font></td>");
                    sb.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + orderline.status.ToString() + "</font></td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                return sb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

    }
}