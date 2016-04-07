using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication5.Models
{
    public class Order
    {
        /// <summary>
        /// 訂單編號
        /// </summary>

        [DisplayName("訂單編號")]
        [Required()]
        public int OrderId { get; set; }



        /// <summary>
        /// 客戶名稱
        /// </summary>
        [DisplayName("客戶名稱")]
        public string CustName { get; set; }



        /// <summary>
        /// 業務(員工姓名)
        /// </summary>
        /// 
        [DisplayName("負責員工")]
        public string EmpName { get; set; }



        /// <summary>
        /// 出貨公司名稱
        /// </summary>
        /// 
        [DisplayName("出貨公司")]
        public string ShipperName { get; set; }


        /// <summary>
        /// 訂單日期
        /// </summary>
        /// 
        [DisplayName("訂購日期")]
        public DateTime? Orderdate { get; set; }



        /// <summary>
        /// 出貨日期
        /// </summary>
        /// 
        [DisplayName("出貨日期")]
        public DateTime? ShippedDate { get; set; }


        /// <summary>
        /// 需要日期
        /// </summary>
        /// 
        [DisplayName("需要日期")]
        public DateTime? RequireDdate { get; set; }


    }
}