using BCMY.WebAPI.Models.UnityDI;
using BCMY.WebAPI.Util;
using DataAccess_EF.EntityFramework;
using DataAccess_EF.ViewModels;
using GenericRepository_UnitOfWork.GR;
using GenericRepository_UnitOfWork.UOW;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BCMY.WebAPI.Controllers
{
    [EnableCors(origins: "https://localhost:44301", headers: "*", methods: "*")]
    public class DiscrepancyController : ApiController
    {
        ObjectProvider objectProvider = null;
        UnitOfWork unitOfWork = null;
        GenericRepository<TblStockUnavailableReason> stockUnavailableReasonRepository = null;
        GenericRepository<TblConfirmedOrderlineChange> confirmedOrderlineChangeRepository = null;

        public DiscrepancyController()
        {
            objectProvider = objectProvider == null ? new ObjectProvider() : objectProvider;
            unitOfWork = unitOfWork == null ? objectProvider.UnitOfWork : unitOfWork;
            stockUnavailableReasonRepository = stockUnavailableReasonRepository == null ? unitOfWork.StockUnavailableReasonRepository : stockUnavailableReasonRepository;
            confirmedOrderlineChangeRepository = confirmedOrderlineChangeRepository == null ? unitOfWork.ConfirmedOrderlineChangeRepository : confirmedOrderlineChangeRepository;
        }

        // used to return stock unavilable reasons
        [HttpGet, ActionName("GetStockUnavailableReasons")]
        public IEnumerable<TblStockUnavailableReason> GetStockUnavilableReasons()
        {
            IEnumerable<TblStockUnavailableReason> unavilableReasons = null;
            try
            {
                unavilableReasons = stockUnavailableReasonRepository.GetAll();                
            }
            catch (Exception ex)
            {
                unavilableReasons = null;
            }
            return unavilableReasons;
        }

        // used to make changes to a confirmed orderline
        [HttpPost, ActionName("RecordConfirmedOrderlineChange")]
        public IEnumerable<OrderLineViewModel> RecordConfirmedOrderlineChange(int productListId, int availableStock,
            string status, int orderIdVal, int orderlineId, string model, int customerId, string customerName, int conatctId,
            string contactFullName, int promisedQuantity, decimal promisedPricePerItem, decimal promisedTotal, int newQuantity, decimal newPricePerItem,
            decimal newTotal, string updateStockStatus, int updateCount, string userRecorded)
        {
            IEnumerable<OrderLineViewModel> orderlines = null;
            try
            {
                var result = confirmedOrderlineChangeRepository.SQLQuery<OrderLineViewModel>(
                    "SP_SaveOrderlineDiscrepancyChange @productListId, @status, " + 
                    "@orderIdVal ,@orderlineId ,@model ,@customerId,@customerName ," + 
                    "@conatctId ,@contactFullName ,@promisedQuantity ,@promisedPricePerItem ,@promisedTotal ,@newQuantity ," + 
                    "@newPricePerItem ,@newTotal ,@updateStockStatus ,@updateCount ,@userRecorded ",
                        new SqlParameter("productListId", SqlDbType.Int) { Value = productListId },
                        new SqlParameter("status", SqlDbType.VarChar) { Value = status },

                        new SqlParameter("orderIdVal", SqlDbType.Int) { Value = orderIdVal },
                        new SqlParameter("orderlineId", SqlDbType.Int) { Value = orderlineId },
                        new SqlParameter("model", SqlDbType.VarChar) { Value = model },
                        new SqlParameter("customerId", SqlDbType.Int) { Value = customerId },
                        new SqlParameter("customerName", SqlDbType.VarChar) { Value = customerName },

                        new SqlParameter("conatctId", SqlDbType.Int) { Value = conatctId },
                        new SqlParameter("contactFullName", SqlDbType.VarChar) { Value = contactFullName },
                        new SqlParameter("promisedQuantity", SqlDbType.Decimal) { Value = promisedQuantity },
                        new SqlParameter("promisedPricePerItem", SqlDbType.Decimal) { Value = promisedPricePerItem },
                        new SqlParameter("promisedTotal", SqlDbType.Decimal) { Value = promisedTotal },
                        new SqlParameter("newQuantity", SqlDbType.Int) { Value = newQuantity },

                        new SqlParameter("newPricePerItem", SqlDbType.Decimal) { Value = newPricePerItem },
                        new SqlParameter("newTotal", SqlDbType.Decimal) { Value = newTotal },
                        new SqlParameter("updateStockStatus", SqlDbType.VarChar) { Value = updateStockStatus },
                        new SqlParameter("updateCount", SqlDbType.Int) { Value = updateCount },
                        new SqlParameter("userRecorded", SqlDbType.VarChar) { Value = userRecorded });
                orderlines = result.ToList<OrderLineViewModel>();
                
                EmailZeroOrNegetiveStocks(availableStock, customerId, customerName, conatctId, contactFullName, updateStockStatus, updateCount, userRecorded, productListId, model);
            }
            catch (Exception ex)
            {
                orderlines = null;
            }
            return orderlines;
        }

        // email when stock goes to zero or below
        private void EmailZeroOrNegetiveStocks(int availableStock, int customerId, string customerName, int conatctId, string contactFullName, string updateStockStatus, int updateCount, string userRecorded,
            int productListId, string model)
        {
            try
            {
                if (updateStockStatus == "Discard from Stock" && availableStock - updateCount <= 0)
                {
                    string message = string.Format("Stock count is needed on below \nProduct Id : {0}\nModel : {1}\nPreviously available stock : {2}\nStock used in order : {3}\n" + 
                        "Customer : {4}\nContact : {5}\nTime stamp : {6}\nSales Person : {7}", productListId, model, availableStock, updateCount, customerName, contactFullName,
                        DateTime.Now, userRecorded);
                    Emailer.InformViaEmail("BCMY - Stock House Keeping", message, null, null, ConfigurationManager.AppSettings["StockHouseKeeperEmail"]);
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
