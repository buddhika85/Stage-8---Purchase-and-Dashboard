using BCMY.WebAPI.Models.UnityDI;
using BCMY.WebAPI.Util;
using DataAccess_EF.EntityFramework;
using DataAccess_EF.ViewModels;
using GenericRepository_UnitOfWork.GR;
using GenericRepository_UnitOfWork.UOW;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BCMY.WebAPI.Controllers
{
    [EnableCors(origins: "https://localhost:44301", headers: "*", methods: "*")]
    public class InternalAppController : ApiController
    {
        ObjectProvider objectProvider = null;
        UnitOfWork unitOfWork = null;
        GenericRepository<TblOrder> orderRepository = null;
        GenericRepository<TblOrderLine> orderLineRepository = null;

        public InternalAppController()
        {
            objectProvider = objectProvider == null ? new ObjectProvider() : objectProvider;
            unitOfWork = unitOfWork == null ? objectProvider.UnitOfWork : unitOfWork;
            orderRepository = orderRepository == null ? unitOfWork.OrderRepository : orderRepository;
            orderLineRepository = orderLineRepository == null ? unitOfWork.OrderLineRepository : orderLineRepository;
        }

        public List<TblOrder> GetAll()
        {
            try
            {
                IEnumerable<TblOrder> orders = orderRepository.GetAll();
                List<TblOrder> ordersList = orders.ToList<TblOrder>();
                return ordersList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //[HttpGet, Route("api/InternalApp/GetConfirmedOrderHeaders")]
        //public string GetConfirmedOrderHeaders()
        //{           
        //    string confirmedOrders = null;
        //    try
        //    {
        //        IEnumerable<OrderViewModel> orders = orderRepository.SQLQuery<OrderViewModel>("SP_GetAllOrderConfirmedOrderHeaders", null);
        //        confirmedOrders = new JavaScriptSerializer().Serialize(orders); 
        //    }
        //    catch (Exception exe)
        //    {
        //        confirmedOrders = null;
        //    }
        //    return confirmedOrders;
        //}

        [HttpGet, Route("api/InternalApp/GetConfirmedOrderHeaders")]
        public IEnumerable<OrderViewModel> GetConfirmedOrderHeaders()
        {
            IEnumerable<OrderViewModel> orders = null;
            try
            {
                orders = orderRepository.SQLQuery<OrderViewModel>("SP_GetAllOrderConfirmedOrderHeaders", null);                
            }
            catch (Exception exe)
            {
                orders = null;
            }
            return orders;
        }

        //[HttpGet, Route("api/InternalApp/GetConfirmedOrderHeaders")]
        //public string GetAllOrderlinesbyOrderId(int orderId)
        //{
        //    string confirmedOrderlines = null;
        //    try
        //    {
        //        // call stored procedure via repository
        //        var result = orderLineRepository.SQLQuery<OrderLineViewModel>("SP_GetOrderLinesByOrderId @orderIdVal",
        //            new SqlParameter("orderIdVal", SqlDbType.Int) { Value = orderId });

        //        // convert the result orderlines (by order ID)
        //        IList<OrderLineViewModel> orderlinesofOrder = result.ToList<OrderLineViewModel>();
        //        orderlinesofOrder = CommonBehaviour.FixDateTime(orderlinesofOrder);
        //        confirmedOrderlines = new JavaScriptSerializer().Serialize(orderlinesofOrder); ;
        //    }
        //    catch (Exception exe)
        //    {
        //        confirmedOrderlines = null;
        //    }
        //    return confirmedOrderlines;
        //}

        [HttpGet, Route("api/InternalApp/GetConfirmedOrderlinesByOrderId")]
        public IEnumerable<OrderlineBriefViewModel> GetAllOrderlinesbyOrderId(int orderId)
        {
            IEnumerable<OrderlineBriefViewModel> orderlinesofOrder = null;
            try
            {
                // call stored procedure via repository
                orderlinesofOrder = orderLineRepository.SQLQuery<OrderlineBriefViewModel>("SP_GetOrderLinesByOrderId @orderIdVal",
                    new SqlParameter("orderIdVal", SqlDbType.Int) { Value = orderId });               
            }
            catch (Exception exe)
            {
                orderlinesofOrder = null;
            }
            return orderlinesofOrder;
        }

    }
}
