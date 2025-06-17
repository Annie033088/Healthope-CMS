using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ApiLayer.Models.Order.Response;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增訂單
        /// </summary>
        public (Order order, int errorCodeNumber) AddOrder(Order addOrder, long orderNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addOrder @memberId, @planType, @planId, @method," +
                    " @orderNumber, @errorCode OUTPUT";

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = addOrder.MemberId;
                cmd.Parameters.Add("@planType", SqlDbType.TinyInt).Value = addOrder.PlanType;
                cmd.Parameters.Add("@planId", SqlDbType.Int).Value = addOrder.PlanId;
                cmd.Parameters.Add("@method", SqlDbType.TinyInt).Value = addOrder.Method;
                cmd.Parameters.Add("@orderNumber", SqlDbType.BigInt).Value = orderNumber;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);
                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                errorCodeNumber = (int)errorCodeOutput.Value;
                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    Order order = new Order()
                    {
                        OrderId = dr.IsNull("f_orderId") ? 0 : dr.Field<int>("f_orderId"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };

                    return (order, errorCodeNumber);
                }

                return (null, errorCodeNumber);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 現金付款
        /// </summary>
        public (int errorCodeNumber, DBResponsePayByCashDto dBResponsePayByCashDto) PayByCash(RequestPayByCashDto payByCashDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editOrderStatusAndPayByCash @orderId, @coachId, " +
                    "@updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = payByCashDto.OrderId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = payByCashDto.UpdateTime;

                if (payByCashDto.CoachId == null)
                    cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = payByCashDto.CoachId;

                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(ds);

                errorCodeNumber = (int)errorCodeOutput.Value;
                cmd.Connection.Close();

                if (ds.Tables.Count > 0)
                {
                    DBResponsePayByCashDto response = new DBResponsePayByCashDto
                    {
                        ElectronicInvoiceId = ds.Tables[0].Rows[0].IsNull("f_electronicInvoiceId") ? 0 :
                            ds.Tables[0].Rows[0].Field<int>("f_electronicInvoiceId"),
                        InvoiceNumber = ds.Tables[0].Rows[0].IsNull("f_invoiceNumber") ? string.Empty :
                            ds.Tables[0].Rows[0].Field<string>("f_invoiceNumber"),
                        RandomNumber = ds.Tables[0].Rows[0].IsNull("f_randomNumber") ? string.Empty :
                            ds.Tables[0].Rows[0].Field<string>("f_randomNumber"),
                        TotalAmount = ds.Tables[0].Rows[0].IsNull("f_totalAmount") ? 0 :
                            ds.Tables[0].Rows[0].Field<int>("f_totalAmount"),
                        PlanName = ds.Tables[0].Rows[0].IsNull("f_planName") ? string.Empty :
                            ds.Tables[0].Rows[0].Field<string>("f_planName"),
                        SingleEntryPassId = null,
                        TicketCode = null,
                    };

                    // 若是票劵方案, 取得票劵資訊

                    if (ds.Tables.Count > 1)
                    {
                        response.SingleEntryPassId = ds.Tables[1].Rows[0].IsNull("f_singleEntryPassId") ? 0 :
                                ds.Tables[1].Rows[0].Field<int>("f_singleEntryPassId");
                        response.TicketCode = ds.Tables[1].Rows[0].IsNull("f_ticketCode") ? Guid.Empty :
                                ds.Tables[1].Rows[0].Field<Guid>("f_ticketCode");
                    }

                    return (errorCodeNumber, response);
                }

                return (errorCodeNumber, null);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 刷卡付款
        /// </summary>
        public (int errorCodeNumber, DBResponsePayByCashDto dBResponsePayByCashDto) PayByCard(RequestPayByCardDto payByCardDto)
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得訂單
        /// </summary>
        public ResponseGetOrderListDto GetOrder(RequestGetOrderDto getOrderDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            ResponseGetOrderListDto response = new ResponseGetOrderListDto();
            List<ResponseGetOrderDto> orders = null;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getOrder @state, @method, " +
                    "@sortOrder, @sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getOrderDto.State == null)
                    cmd.Parameters.Add("@state", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@state", SqlDbType.TinyInt).Value = getOrderDto.State;

                if (getOrderDto.Method == null)
                    cmd.Parameters.Add("@method", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@method", SqlDbType.TinyInt).Value = getOrderDto.Method;

                if (getOrderDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getOrderDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getOrderDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getOrderDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getOrderDto.Page;
                SqlParameter totalPageOutput = new SqlParameter("@totalPage", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(totalPageOutput);


                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);
                response.TotalPage = (int)totalPageOutput.Value;

                cmd.Connection.Close();

                if (dt.Rows.Count > 0) orders = new List<ResponseGetOrderDto>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    ResponseGetOrderDto order = new ResponseGetOrderDto()
                    {
                        OrderId = dr.IsNull("f_orderId") ? 0 : dr.Field<int>("f_orderId"),
                        MemberId = dr.IsNull("f_memberId") ? 0 : dr.Field<int>("f_memberId"),
                        MemberName = dr.IsNull("f_memberName") ? string.Empty : dr.Field<string>("f_memberName"),
                        MemberPhone = dr.IsNull("f_memberPhone") ? 0 : dr.Field<int>("f_memberPhone"),
                        PlanType = (byte)(dr.IsNull("f_planType") ? 0 : dr.Field<byte>("f_planType")),
                        PlanName = dr.IsNull("f_planName") ? string.Empty : dr.Field<string>("f_planName"),
                        OrderNumber = dr.IsNull("f_orderNumber") ? 0 : dr.Field<long>("f_orderNumber"),
                        State = (byte)(dr.IsNull("f_state") ? 0 : dr.Field<byte>("f_state")),
                        Amount = dr.IsNull("f_amount") ? 0 : dr.Field<int>("f_amount"),
                        Method = (byte)(dr.IsNull("f_method") ? 0 : dr.Field<byte>("f_method")),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };
                    orders.Add(order);
                }

                response.OrderList = orders;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }
    }
}
