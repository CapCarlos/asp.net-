using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Controllers
{
    public class OrderController : Controller
    {
        Models.CodeService codeService = new Models.CodeService();
        Models.OrderService orderService = new Models.OrderService();
        Models.OrderDetailsService orderDetailsService = new Models.OrderDetailsService();
        /// <summary>
        /// 訂單管理首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.SCodeData = this.codeService.GetShipper(-1);
            return View();
        }




        /// <summary>
        /// 新增訂單畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult InsertOrder()
        {
            ViewBag.CustCodeData = this.codeService.GetCustomer(-1);
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ProductCodeData = this.codeService.GetProduct();
            ViewBag.ShipCodeData = this.codeService.GetShipper(-1);
            
            return View(new Models.Order());
        }


        /// <summary>
        /// 新增訂單存檔的Action
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order)
        {
            if (ModelState.IsValid)
            {
                int a = orderService.InsertOrder(order);
                orderDetailsService.InsertOrderDetail(order.OrderDetails, a);
                return RedirectToAction("Index/" + a);
            }
            return RedirectToAction("InsertOrder");

        }



        /// <summary>
        /// 依訂單ID刪除訂單
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteOrder(string orderId)
        {
            try
            {
                orderService.DeleteOrderById(orderId);
                orderDetailsService.DeleteOrderDetail(orderId);
                return this.Json(true);
            }
            catch (Exception)
            {
                return this.Json(false);
            }
        }

        /// <summary>
        /// 依訂單ID取得訂單資料的畫面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Update(string id)
        {
            Models.Order order = this.orderService.GetOrderById(id);
            order.OrderDetails = this.orderDetailsService.GetOrderByOrderId(id);
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ShipCodeData = this.codeService.GetShipper(-1);
            ViewBag.CustCodeData = this.codeService.GetCustomer(-1);
            ViewBag.OrderDate = string.Format("{0:yyyy-MM-dd}", order.Orderdate);
            ViewBag.RequireDdate = string.Format("{0:yyyy-MM-dd}", order.RequireDdate);
            ViewBag.ShippedDate = string.Format("{0:yyyy-MM-dd}", order.ShippedDate);
            ViewBag.ProductCodeData = this.codeService.GetProduct();
            ViewBag.OrderData = order;
            return View(order);
        }
        /// <summary>
        /// 修改訂單存檔的Action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Update(Models.Order order)
        {
            orderService.UpdateOrder(order);
            orderDetailsService.UpdateOrderDeail(order.OrderDetails, order.OrderId);
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.ShipCodeData = this.codeService.GetShipper(-1);
            return View("Index");
        }



        /// <summary>
        /// 取得訂單查詢結果
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Index(Models.OrderSearchArg arg)
        {
            ViewBag.EmpCodeData = this.codeService.GetEmp(-1);
            ViewBag.SCodeData = this.codeService.GetShipper(-1);
            Models.OrderService orderService = new Models.OrderService();
            ViewBag.SearchResult = orderService.GetOrderByCondtioin(arg);
            return View("Index");
        }

    }
}