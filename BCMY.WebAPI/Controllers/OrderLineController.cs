﻿using BCMY.WebAPI.Models.UnityDI;
using BCMY.WebAPI.Util;
using DataAccess_EF.EntityFramework;
using GenericRepository_UnitOfWork.GR;
using GenericRepository_UnitOfWork.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data;
using System.Data.SqlClient;
using DataAccess_EF.ViewModels;
using System.Net.Http.Headers;
using System.IO;

namespace BCMY.WebAPI.Controllers
{
    [EnableCors(origins: "https://localhost:44301", headers: "*", methods: "*")]
    public class OrderLineController : ApiController
    {
        ObjectProvider objectProvider = null;
        UnitOfWork unitOfWork = null;
        GenericRepository<TblOrderLine> orderLineRepository = null;

        // constructor
        public OrderLineController()
        {
            objectProvider = objectProvider == null ? new ObjectProvider() : objectProvider;
            unitOfWork = unitOfWork == null ? objectProvider.UnitOfWork : unitOfWork;
            orderLineRepository = orderLineRepository == null ? unitOfWork.OrderLineRepository : orderLineRepository;
        }

        /// <summary>
        /// Used to save a negotiation record
        /// </summary>
        /// http://localhost:61945/api/Orderline?productListId=107233&quantityVal=3&pricePerItem=5.0&totalAmountVal=15.0&statusVal=2&orderIdVal=47
        [HttpGet, ActionName("SaveOrderlineWithNegotiation")]
        public IList<OrderLineViewModel> SaveOrderlineWithNegotiation(int productListId, decimal quantityVal, decimal pricePerItem, decimal totalAmountVal, int statusVal, int orderIdVal)
        {            
            try
            {
                // validation 
                if (OrderLineNegotiationValidator.ValidateOrderLineOrNegotiation(productListId, quantityVal, pricePerItem, totalAmountVal, statusVal, orderIdVal))
                {
                    string status = CommonBehaviour.GetCommonStatusString(statusVal);
                    // call stored procedure via repository
                    var result = orderLineRepository.SQLQuery<OrderLineViewModel>("SP_SaveOrderLineWithNegotiation @productListId, @quantityVal, @pricePerItem, @totalAmountVal, @status, @dateTime, @orderIdVal",
                        new SqlParameter("productListId", SqlDbType.Int) { Value = productListId },
                        new SqlParameter("quantityVal", SqlDbType.Int) { Value = quantityVal }, 
                        new SqlParameter("pricePerItem", SqlDbType.Decimal) { Value = pricePerItem },
                        new SqlParameter("totalAmountVal", SqlDbType.Decimal) { Value = totalAmountVal },
                        new SqlParameter("status", SqlDbType.Text) { Value = status },
                        new SqlParameter("dateTime", SqlDbType.DateTime) { Value = DateTime.Now },
                        new SqlParameter("orderIdVal", SqlDbType.Int) { Value = orderIdVal });

                    // convert the result orderlines (by order ID)
                    IList<OrderLineViewModel> orderLinesOfOrder = result.ToList<OrderLineViewModel>();
                    orderLinesOfOrder = CommonBehaviour.FixDateTime(orderLinesOfOrder);
                    return orderLinesOfOrder;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }        

        /// <summary>
        /// Gets all the orderlines of an order by an orderId
        /// </summary>
        /// http://localhost:61945/api/Orderline?orderIdVal=47
        [HttpGet, ActionName("GetAllOrderlinesByOrderId")]
        public IList<OrderLineViewModel> GetAllOrderlinesByOrderId(int orderIdVal)
        {
            try
            { 
                // call stored procedure via repository
                var result = orderLineRepository.SQLQuery<OrderLineViewModel>("SP_GetOrderLinesByOrderId @orderIdVal",                       
                    new SqlParameter("orderIdVal", SqlDbType.Int) { Value = orderIdVal });

                // convert the result orderlines (by order ID)
                IList<OrderLineViewModel> orderLinesOfOrder = result.ToList<OrderLineViewModel>();
                orderLinesOfOrder = CommonBehaviour.FixDateTime(orderLinesOfOrder);
                return orderLinesOfOrder;
            }
            catch (Exception ex)
            {
                return null;
            }
        } 

        // Returns all conditions by categoryIds comma list
        [HttpPost, ActionName("GenerateOrderlineInfoReport")]
        public HttpResponseMessage GenerateOrderlineInfoReport(int orderIdValForReport)
        {
            try
            {
                // call stored procedure via repository
                var result = orderLineRepository.SQLQuery<OrderLineViewModel>("SP_GetOrderLinesByOrderId @orderIdVal",
                    new SqlParameter("orderIdVal", SqlDbType.Int) { Value = orderIdValForReport });

                // convert the result orderlines (by order ID)
                IEnumerable<OrderLineViewModel> orderLinesOfOrder = result.ToList<OrderLineViewModel>();

                // order details
                var resultOrder = orderLineRepository.SQLQuery<OrderViewModel>("SP_GetOrderVmById @orderId",
                    new SqlParameter("orderId", SqlDbType.Int) { Value = orderIdValForReport });
                OrderViewModel orderInfo = resultOrder.SingleOrDefault<OrderViewModel>();

                string path = new ExcelWriter().GenerateOrderlineInfoExcelReport(orderLinesOfOrder, orderInfo);
                HttpResponseMessage resultH = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(path, FileMode.Open);
                resultH.Content = new StreamContent(stream);
                resultH.Content.Headers.ContentType = 
                    new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return resultH;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get orderline info by orderline Id
        /// </summary>
        /// http://localhost:61945/api/Orderline?orderlineId=1&orderCurrency=GBP
        [HttpGet, ActionName("GetOrderLineInfoByOrderlineId")]
        public OrderLineViewModel GetOrderLineInfoByOrderlineId(int orderlineId, string orderCurrency)
        {
            try
            {
                
                // call stored procedure via repository
                var result = orderLineRepository.SQLQuery<OrderLineViewModel>("SP_GetOrderlineInfoById @orderlineId, @orderCurrency",
                    new SqlParameter("orderlineId", SqlDbType.Int) { Value = orderlineId },
                    new SqlParameter("orderCurrency", SqlDbType.VarChar) { Value = orderCurrency });

                // convert the result to orderline
                OrderLineViewModel olVm = result.FirstOrDefault<OrderLineViewModel>();
                return olVm;                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // returns orderline and oredr info by orderline Id
        public OrderlineAndOrderInfoViewModel GetOrderlineAndOrderInfo(int orderlineIdForDf)
        {
            OrderlineAndOrderInfoViewModel orderlineOrderInfoVm = null;
            try
            {
                var result = orderLineRepository.SQLQuery<OrderlineAndOrderInfoViewModel>("SP_GetOrderlineAndOrderDetails @orderlineId",
                    new SqlParameter("orderlineId", SqlDbType.Int) { Value = orderlineIdForDf });
                orderlineOrderInfoVm = result.SingleOrDefault<OrderlineAndOrderInfoViewModel>();
            }
            catch (Exception ex)
            {
                orderlineOrderInfoVm = null;
            }
            return orderlineOrderInfoVm;
        }

        /// <summary>
        /// Used to change the status of the order to - confirm 
        /// Returns a string message explaining the result
        /// http://localhost:61945/api/Order?orderId=25
        /// </summary>
        [HttpGet, ActionName("ConfirmOrderLinesWithOrder")]
        public string ConfirmOrder(int orderId)
        {
            try
            {
                // call stored procedure via repository
                var result = orderLineRepository.SQLQuery<string>("SP_ConfirmOrderLinesWithOrder @orderId",
                    new SqlParameter("orderId", SqlDbType.Int) { Value = orderId });

                return result.FirstOrDefault<string>();
            }
            catch (Exception ex)
            {
                return "Error - Could not access the DB Server - Please contact IT Support";
            }
        }

        /// <summary>
        /// Used to change the status of the order to - confirm 
        /// Returns a string message explaining the result
        /// http://localhost:61945/api/orderline?orderId=25&orderlineId=125&deleteOrReject=del or
        /// http://localhost:61945/api/orderline?orderId=25&orderlineId=125&deleteOrReject=rej
        /// </summary>
        [HttpGet, ActionName("DeleteRejectOrderline")]
        public string DeleteOrderline(string deleteOrReject, int orderlineId, int orderId)
        {
            try
            {
                // call stored procedure via repository
                var result = orderLineRepository.SQLQuery<string>("SP_DeleteOrRejectOrderline @deleteOrReject, @orderlineId, @orderId",
                            new SqlParameter("deleteOrReject", SqlDbType.VarChar) { Value = deleteOrReject },
                            new SqlParameter("orderlineId", SqlDbType.Int) { Value = orderlineId },
                            new SqlParameter("orderId", SqlDbType.Int) { Value = orderId });

                return result.FirstOrDefault<string>();
            }
            catch (Exception ex)
            {
                return "Error - Could not access the DB Server - Please contact IT Support";
            }
        }

    }
}
