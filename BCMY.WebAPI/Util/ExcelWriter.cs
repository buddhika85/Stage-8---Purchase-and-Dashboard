using DataAccess_EF.ViewModels;
using ExcelLibrary.SpreadSheet;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace BCMY.WebAPI.Util
{
    public class ExcelWriter
    {
        // used to generate an excel file for product stock info excel file download
        public string GenerateModelStockInfoExcelReport(IEnumerable<ProductStockInfoViewModel> stockInfoModels)
        {
            try
            {
                // Set the file name and get the output directory
                string fileName = "ProductStockInfo-" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
                var outputDir = HttpContext.Current.Server.MapPath("~/App_Data/Excel");

                // Create the file using the FileInfo object
                FileInfo file = new FileInfo(outputDir + fileName);

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(fileName);
                    
                    // header    
                    int rowNumber = 1;
                    worksheet.Cells[rowNumber, 1, rowNumber, 5].Style.Font.Bold = true;
                    worksheet.Cells[rowNumber, 1, rowNumber, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowNumber, 1, rowNumber, 5].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    worksheet.Cells[rowNumber, 1].Value = "Product list Id";
                    worksheet.Cells[rowNumber, 2].Value = "Quantity";
                    worksheet.Cells[rowNumber, 3].Value = "Category";                    
                    worksheet.Cells[rowNumber, 4].Value = "Condition";                    
                    worksheet.Cells[rowNumber, 5].Value = "Brand";

                    foreach (ProductStockInfoViewModel modelData in stockInfoModels)
                    {
                        ++rowNumber;
                        worksheet.Cells[rowNumber, 1].Value = modelData.ProductListId;
                        worksheet.Cells[rowNumber, 2].Value = modelData.Quantity;
                        worksheet.Cells[rowNumber, 3].Value = modelData.Category;
                        worksheet.Cells[rowNumber, 4].Value = modelData.Condition;
                        worksheet.Cells[rowNumber, 5].Value = modelData.Brand;
                    }
                    worksheet.Column(1).AutoFit();
                    worksheet.Column(2).AutoFit();
                    worksheet.Column(3).AutoFit();
                    worksheet.Column(4).AutoFit();
                    worksheet.Column(5).AutoFit();
                    package.Save();
                }
                return file.FullName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        // used to generate the excel file for orderline info excel file download
        public string GenerateOrderlineInfoExcelReport(IEnumerable<OrderLineViewModel> orderlines, OrderViewModel orderInfo)
        {
            try
            {
                // Set the file name and get the output directory
                string fileName = "OrderInfo-" +  orderInfo.id + "-" + DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss") + ".xlsx";
                var outputDir = HttpContext.Current.Server.MapPath("~/App_Data/Excel");

                // Create the file using the FileInfo object
                FileInfo file = new FileInfo(outputDir + fileName);

                // Create the package and make sure you wrap it in a using statement
                using (var package = new ExcelPackage(file))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(fileName);
                    int rowNumber = 1;
                    worksheet.Cells[rowNumber, 1].Value = "Order ID ";
                    worksheet.Cells[rowNumber, 2].Value = orderInfo.id;
                    worksheet.Cells[++rowNumber, 1].Value = "Order Status ";
                    worksheet.Cells[rowNumber, 2].Value = orderInfo.status;
                    worksheet.Cells[++rowNumber, 1].Value = "Company ";
                    worksheet.Cells[rowNumber, 2].Value = orderInfo.company;
                    worksheet.Cells[++rowNumber, 1].Value = "Contact ";
                    worksheet.Cells[rowNumber, 2].Value = orderInfo.contactFulName;
                    worksheet.Cells[++rowNumber, 1].Value = "Order date ";
                    worksheet.Cells[rowNumber, 2].Value = orderInfo.orderCreationDate;                   
                    worksheet.Cells[++rowNumber, 1].Value = "Currency ";
                    worksheet.Cells[rowNumber, 2].Value = orderInfo.currency;
                    worksheet.Cells[++rowNumber, 1].Value = "Order total ";
                    worksheet.Cells[rowNumber, 2].Value = CommonBehaviour.GetCurrencySymbol(orderInfo.currency) + " " + orderInfo.total;

                    // header 
                    rowNumber = rowNumber + 2;
                    worksheet.Cells[rowNumber, 1, rowNumber, 6].Style.Font.Bold = true;
                    worksheet.Cells[rowNumber, 1, rowNumber, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;                 
                    worksheet.Cells[rowNumber, 1, rowNumber, 6].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    worksheet.Cells[rowNumber, 1].Value = "Condition";
                    worksheet.Cells[rowNumber, 2].Value = "Brand";
                    worksheet.Cells[rowNumber, 3].Value = "Model";
                    worksheet.Cells[rowNumber, 4].Value = "Quantity";
                    worksheet.Cells[rowNumber, 5].Value = "PPI";
                    worksheet.Cells[rowNumber, 5].Value = "Total(" + CommonBehaviour.GetCurrencySymbol(orderInfo.currency) + ")";
                    worksheet.Cells[rowNumber, 6].Value = "Status";

                    foreach (OrderLineViewModel orderline in orderlines)
                    {
                        ++rowNumber;
                        worksheet.Cells[rowNumber, 1].Value = orderline.condition;
                        worksheet.Cells[rowNumber, 2].Value = orderline.brand;
                        worksheet.Cells[rowNumber, 3].Value = orderline.model;
                        worksheet.Cells[rowNumber, 4].Value = orderline.quantity;
                        worksheet.Cells[rowNumber, 5].Value = orderline.negotiatedPricePerItem;
                        worksheet.Cells[rowNumber, 5].Value = orderline.totalAmount;
                        worksheet.Cells[rowNumber, 6].Value = orderline.status;
                    }
                    worksheet.Column(1).AutoFit();
                    worksheet.Column(2).AutoFit();
                    worksheet.Column(3).AutoFit();
                    worksheet.Column(4).AutoFit();
                    worksheet.Column(5).AutoFit();
                    worksheet.Column(6).AutoFit();
                    package.Save();
                }
                return file.FullName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region ExcelLibrary
        //// used to generate an excel file for product stock info excel file download
        //public string GenerateModelStockInfoExcelReport(IEnumerable<ProductStockInfoViewModel> stockInfoModels)
        //{
        //    try
        //    {
        //        string fileName = "ProductStockInfo.xlsx"; // string.Format("ProductStockInfo-{0}.xlsx", DateTime.Now).Replace('/', '_');
        //        string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Excel");
        //        filePath = string.Format("{0}\\{1}", filePath, fileName);
        //        Workbook workbook = new Workbook();
        //        Worksheet worksheet = new Worksheet(fileName);
        //        int rowNumber = 1;
        //        // header            
        //        worksheet.Cells[rowNumber, 1] = new Cell("Product list Id");
        //        //worksheet.Cells[rowNumber, 1].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 2] = new Cell("Quantity");
        //        //worksheet.Cells[rowNumber, 2].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 3] = new Cell("Category");
        //        //worksheet.Cells[rowNumber, 3].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 4] = new Cell("Condition");
        //        //worksheet.Cells[rowNumber, 4].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 5] = new Cell("Brand");
        //        //worksheet.Cells[rowNumber, 5].Style.BackColor = Color.LightBlue;
        //        // data
        //        foreach (ProductStockInfoViewModel modelData in stockInfoModels)
        //        {
        //            ++rowNumber;
        //            worksheet.Cells[rowNumber, 1] = new Cell(modelData.ProductListId);                
        //            worksheet.Cells[rowNumber, 2] = new Cell(modelData.Quantity);                
        //            worksheet.Cells[rowNumber, 3] = new Cell(modelData.Category);                
        //            worksheet.Cells[rowNumber, 4] = new Cell(modelData.Condition);                
        //            worksheet.Cells[rowNumber, 5] = new Cell(modelData.Brand);                
        //        }
        //        workbook.Worksheets.Add(worksheet);
        //        workbook.Save(filePath);
        //        return filePath;
        //    }
        //    catch (Exception ex)
        //    {                
        //        throw ex;
        //    }
        //}

        //// used to generate the excel file for orderline info excel file download
        //public string GenerateOrderlineInfoExcelReport(IEnumerable<OrderLineViewModel> orderlines, OrderViewModel orderInfo)
        //{
        //    try
        //    {
        //        string fileName = "OrderInfo.xlsx"; // string.Format("OrderInfo-{0}-{1}.xlsx", DateTime.Now, orderInfo.id).Replace('/', '_');
        //        string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Excel");
        //        filePath = string.Format("{0}\\{1}", filePath, fileName);
        //        Workbook workbook = new Workbook();
        //        Worksheet worksheet = new Worksheet(fileName);
        //        int rowNumber = 1;
        //        worksheet.Cells[rowNumber, 1] = new Cell("Order ID ");
        //        worksheet.Cells[rowNumber, 2] = new Cell(orderInfo.id);
        //        worksheet.Cells[++rowNumber, 1] = new Cell("Order Status ");
        //        worksheet.Cells[rowNumber, 2] = new Cell(orderInfo.status);
        //        worksheet.Cells[++rowNumber, 1] = new Cell("Company ");
        //        worksheet.Cells[rowNumber, 2] = new Cell(orderInfo.company);
        //        worksheet.Cells[++rowNumber, 1] = new Cell("Contact ");
        //        worksheet.Cells[rowNumber, 2] = new Cell(orderInfo.contactFulName);
        //        worksheet.Cells[++rowNumber, 1] = new Cell("Order date ");
        //        worksheet.Cells[rowNumber, 2] = new Cell(orderInfo.orderCreationDate);
        //        worksheet.Cells[++rowNumber, 1] = new Cell("Past order ");
        //        worksheet.Cells[rowNumber, 2] = new Cell(orderInfo.pastOrder);
        //        worksheet.Cells[++rowNumber, 1] = new Cell("Currency ");
        //        worksheet.Cells[rowNumber, 2] = new Cell(orderInfo.currency);
        //        worksheet.Cells[++rowNumber, 1] = new Cell("Order total ");
        //        worksheet.Cells[rowNumber, 2] = new Cell(CommonBehaviour.GetCurrencySymbol(orderInfo.currency) + " " + orderInfo.total);
        //        // headers
        //        rowNumber = rowNumber + 2;
        //        worksheet.Cells[rowNumber, 1] = new Cell("Condition");
        //        //worksheet.Cells[rowNumber, 1].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 2] = new Cell("Brand");
        //        //worksheet.Cells[rowNumber, 2].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 3] = new Cell("Model");
        //        //worksheet.Cells[rowNumber, 3].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 4] = new Cell("Quantity");
        //        //worksheet.Cells[rowNumber, 4].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 5] = new Cell("PPI");
        //        //worksheet.Cells[rowNumber, 5].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 5] = new Cell("Total(" + CommonBehaviour.GetCurrencySymbol(orderInfo.currency) + ")");
        //        //worksheet.Cells[rowNumber, 5].Style.BackColor = Color.LightBlue;
        //        worksheet.Cells[rowNumber, 6] = new Cell("Status");
        //        //worksheet.Cells[rowNumber, 6].Style.BackColor = Color.LightBlue;
        //        //data
        //        foreach (OrderLineViewModel orderline in orderlines)
        //        {
        //            ++rowNumber;
        //            worksheet.Cells[rowNumber, 1] = new Cell(orderline.condition);                    
        //            worksheet.Cells[rowNumber, 2] = new Cell(orderline.brand);                    
        //            worksheet.Cells[rowNumber, 3] = new Cell(orderline.model);                    
        //            worksheet.Cells[rowNumber, 4] = new Cell(orderline.quantity);                    
        //            worksheet.Cells[rowNumber, 5] = new Cell(orderline.negotiatedPricePerItem);                    
        //            worksheet.Cells[rowNumber, 5] = new Cell(orderline.totalAmount);                    
        //            worksheet.Cells[rowNumber, 6] = new Cell(orderline.status);                    
        //        }
        //        workbook.Worksheets.Add(worksheet);
        //        workbook.Save(filePath);
        //        return filePath;
        //    }
        //    catch (Exception ex)
        //    {                
        //        throw ex;
        //    }            
        //} 
        #endregion 

    }
}