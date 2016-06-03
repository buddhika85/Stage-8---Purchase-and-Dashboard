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

    }
}