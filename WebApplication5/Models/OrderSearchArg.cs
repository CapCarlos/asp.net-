using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class OrderSearchArg
    {

        public string OrderId { get; set; }
        public string EmployeeID { get; set; }
        public string CustomerID { get; set; }
        public string ShipperID { get; set; }
        public string Orderdate { get; set; }
        public string ShippedDate { get; set; }
        public string RequireDdate { get; set; }
        
        
    }
}