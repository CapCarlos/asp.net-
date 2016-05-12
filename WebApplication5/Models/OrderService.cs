using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication5.Models
{
    public class OrderService
    {


        /// <summary>
		/// 取得DB連線字串
		/// </summary>
		/// <returns></returns>
		private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }


        /// <summary>
        /// 新增訂單
        /// </summary>
        public int InsertOrder(Models.Order order)
        {
            string sql = @"INSERT INTO Sales.Orders(
	                        CustomerID,EmployeeID,orderdate,requireddate,shippeddate,shipperid,freight,
	                        shipname,shipaddress,shipcity,shipregion,shippostalcode,shipcountry
                        )VALUES(
							@CustomerID,@EmployeeID,@Orderdate,@RequireDdate,@ShippedDate,@ShipperID,@Freight,
							@ShipName,@ShipAddress,@ShipCity,@ShipRegion,@ShipPostalCode,@ShipCountry
						)
						SELECT SCOPE_IDENTITY()
						";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", order.RequireDdate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));

                orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return orderId;
        }


        /// <summary>
        /// 刪除訂單 By orderId
        /// </summary>
        public void DeleteOrderById(String orderId)
        {
            try
            {
                string sql = "DELETE FROM Sales.Orders Where orderid=@orderid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderid", orderId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        /// <summary>
        /// 依照條件取得訂單資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> GetOrderByCondtioin(Models.OrderSearchArg arg)
        {

            DataTable dt = new DataTable();
            string sql = @"SELECT 
					A.OrderId,B.CustomerID,B.Companyname,
					C.lastname+ C.firstname As EmpName,C.EmployeeID,
					A.Orderdate,A.RequireDdate,A.ShippedDate,
					D.CompanyName AS Sname,D.ShipperID ,
					A.ShipName
					From Sales.Orders As A 
					JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					JOIN Sales.Shippers As D ON A.shipperid=D.shipperid
					Where (B.CustomerID Like '%'+@CustomerID+'%' Or @CustomerID='') AND 
						  (A.Orderdate=@Orderdate Or @Orderdate='') AND (A.OrderId=@OrderId Or @OrderId='') AND (C.EmployeeID=@EmployeeID Or @EmployeeID='') AND (D.ShipperID=@ShipperID Or @ShipperID='') ";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", arg.OrderId == null ? string.Empty : arg.OrderId));
                cmd.Parameters.Add(new SqlParameter("@CustomerID", arg.CustomerID == null ? string.Empty : arg.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", arg.EmployeeID == null ? string.Empty : arg.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", arg.ShipperID == null ? string.Empty : arg.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", arg.Orderdate == null ? string.Empty : arg.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", arg.ShippedDate == null ? string.Empty : arg.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@RequireDdate", arg.RequireDdate == null ? string.Empty : arg.RequireDdate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }


            return this.MapOrderDataToList(dt);
        }

        private List<Models.Order> MapOrderDataToList(DataTable orderData)
        {
            List<Models.Order> result = new List<Order>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Order()
                {

                    CustomerID = row["Companyname"].ToString(),

                    EmployeeID = row["EmpName"].ToString(),
                   
                    Orderdate = row["Orderdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Orderdate"],
                    OrderId = (int)row["OrderId"],
                    RequireDdate = row["RequireDdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequireDdate"],
                   
                    ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"],

                    ShipperID = row["Sname"].ToString()
                });
            }
            return result;
        }
    }
}