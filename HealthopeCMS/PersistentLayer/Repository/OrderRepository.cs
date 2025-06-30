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
        public (int errorCodeNumber, DBResponsePaymentDto dBResponsePaymentDto) PayByCash(RequestPayByCashDto payByCashDto)
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
                    DBResponsePaymentDto response = new DBResponsePaymentDto
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
        public (int errorCodeNumber, DBResponsePaymentDto dBResponsePaymentDto) PayByCardSuccess(RequestPayByCardDto payByCardDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editOrderStatusPayByCardSuccess @orderId, @coachId, @errorCode OUTPUT";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = payByCardDto.OrderId;

                if (payByCardDto.CoachId == null)
                    cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = payByCardDto.CoachId;

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
                    DBResponsePaymentDto response = new DBResponsePaymentDto
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
                        OrderNumber = dr.IsNull("f_orderNumber") ? string.Empty : dr.Field<long>("f_orderNumber").ToString(),
                        State = (byte)(dr.IsNull("f_state") ? 0 : dr.Field<byte>("f_state")),
                        Amount = dr.IsNull("f_amount") ? 0 : dr.Field<int>("f_amount"),
                        Method = (byte)(dr.IsNull("f_method") ? 0 : dr.Field<byte>("f_method")),
                        InvoiceStatus = (byte)(dr.IsNull("f_invoiceStatus") ? 0 : dr.Field<byte>("f_invoiceStatus")),
                        Remark = dr.IsNull("f_remark") ? string.Empty : dr.Field<string>("f_remark"),
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

        /// <summary>
        /// 新增信用卡交易紀錄 (待付款)
        /// </summary>
        public (CreditCardTransaction creditCardTransaction, int errorCodeNumber) AddCreditCardTransaction(RequestPayByCardDto payByCardDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addCreditCardTransaction @orderId, @updateTime, " +
                    "@coachId, @errorCode OUTPUT";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = payByCardDto.OrderId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = payByCardDto.UpdateTime;

                if (payByCardDto.CoachId == null)
                {
                    cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = payByCardDto.CoachId;
                }

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
                    CreditCardTransaction creditCardTransaction = new CreditCardTransaction()
                    {
                        CreditCardTransactionId = dr.IsNull("f_creditCardTransactionId") ?
                            0 : dr.Field<int>("f_creditCardTransactionId"),
                        Amount = dr.IsNull("f_amount") ? 0 : dr.Field<int>("f_amount"),
                    };

                    return (creditCardTransaction, errorCodeNumber);
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
        /// 根據 id 取得訂單
        /// </summary>
        public (Order order, List<OrderState> orderStates) GetOrderDetailById(int orderId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            Order order = new Order();
            List<OrderState> orderStates = new List<OrderState>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getOrderDetailById @orderId";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Connection.Close();
                if (ds.Tables.Count > 0)
                {
                    // 取得訂單細項
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        order.OrderNumber = ds.Tables[0].Rows[i].IsNull("f_orderNumber") ? 0 :
                            ds.Tables[0].Rows[i].Field<long>("f_orderNumber");
                        order.PlanName = ds.Tables[0].Rows[i].IsNull("f_planName") ?
                            string.Empty : ds.Tables[0].Rows[i].Field<string>("f_planName");
                        order.Remark = ds.Tables[0].Rows[i].IsNull("f_remark") ?
                            string.Empty : ds.Tables[0].Rows[i].Field<string>("f_remark");
                        order.Amount = ds.Tables[0].Rows[i].IsNull("f_amount") ? 0 :
                            ds.Tables[0].Rows[i].Field<int>("f_amount");
                        order.State = (byte)(ds.Tables[0].Rows[i].IsNull("f_state") ? 0 :
                            ds.Tables[0].Rows[i].Field<byte>("f_state"));
                        order.Method = (byte)(ds.Tables[0].Rows[i].IsNull("f_method") ? 0 :
                            ds.Tables[0].Rows[i].Field<byte>("f_method"));
                        order.CreateTime = ds.Tables[0].Rows[i].IsNull("f_createTime") ?
                            DateTime.MinValue : ds.Tables[0].Rows[i].Field<DateTime>("f_createTime");
                    }

                    // 取得訂單狀態
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        OrderState orderState = new OrderState();
                        orderState.OrderStateId =
                            ds.Tables[1].Rows[i].IsNull("f_orderStateId") ? 0 :
                            ds.Tables[1].Rows[i].Field<int>("f_orderStateId");
                        orderState.Remark = ds.Tables[1].Rows[i].IsNull("f_remark") ?
                            string.Empty : ds.Tables[1].Rows[i].Field<string>("f_remark");
                        orderState.State = (byte)(ds.Tables[1].Rows[i].IsNull("f_state") ? 0 :
                            ds.Tables[1].Rows[i].Field<byte>("f_state"));
                        orderState.CreateTime = ds.Tables[1].Rows[i].IsNull("f_createTime") ?
                            DateTime.MinValue : ds.Tables[1].Rows[i].Field<DateTime>("f_createTime");
                        orderState.UpdateTime = ds.Tables[1].Rows[i].IsNull("f_updateTime") ?
                            DateTime.MinValue : ds.Tables[1].Rows[i].Field<DateTime>("f_updateTime");

                        orderStates.Add(orderState);
                    }

                    return (order, orderStates);
                }

                return (null, null);
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
        /// 修改訂單狀態備註
        /// </summary>
        public bool EditOrderStateRemark(OrderState orderState)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editOrderStateRemark @orderStateId, @remark, @updateTime";

                cmd.Parameters.Add("@orderStateId", SqlDbType.Int).Value = orderState.OrderStateId;
                cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = orderState.Remark;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = orderState.UpdateTime;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                // 受影響筆數>0代表成功
                if (ExeCnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        /// 修改訂單備註
        /// </summary>
        public bool EditOrderRemark(Order order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editOrderRemark @orderId, @remark, @updateTime";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = order.OrderId;
                cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = order.Remark;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = order.UpdateTime;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                // 受影響筆數>0代表成功
                if (ExeCnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        /// 修改訂單狀態：待付款 => 取消
        /// </summary>
        public bool CancelPendingOrder(Order order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editOrderStateCancelPendingOrder @orderId, @updateTime";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = order.OrderId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = order.UpdateTime;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                // 受影響筆數>0代表成功
                if (ExeCnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        /// 訂單 7 日內無條件退款
        /// </summary>
        public (int errorCodeNumber, string invoiceNumber) RefundIn7Days(Order order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editOrderStateRefundIn7Days @orderId, @updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = order.OrderId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = order.UpdateTime;
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
                    string inoviceNumber = dr.IsNull("f_invoiceNumber") ? string.Empty : dr.Field<string>("f_invoiceNumber");

                    return (errorCodeNumber, inoviceNumber);
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
        /// 確認是否可以無條件退費 若是=>請前端管理者確認是否要解約而不是無條件退費, 若否=>直接走解約流程
        /// </summary>
        public (int errorCodeNumber, bool haveRefundQualify) CheckoutUnconditionalRefundQualify(Order order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getUnconditionalRefundQualify @orderId, @errorCode OUTPUT";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = order.OrderId ;
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
                    bool haveRefundQualify = dr.IsNull("f_haveRefundQualify") ? false : dr.Field<bool>("f_haveRefundQualify");

                    return (errorCodeNumber, haveRefundQualify);
                }

                return (errorCodeNumber, false);
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
        /// 解約訂單
        /// </summary>
        public (int errorCodeNumber, string invoiceNumber) TerminateOrder(Order order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editOrderStateTerminate @orderId, @updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = order.OrderId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = order.UpdateTime;
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
                    string inoviceNumber = dr.IsNull("f_invoiceNumber") ? string.Empty : dr.Field<string>("f_invoiceNumber");

                    return (errorCodeNumber, inoviceNumber);
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
        /// 違約訂單
        /// </summary>
        public (int errorCodeNumber, string invoiceNumber, DBResponsePrintInvoiceDto dbResponse) BreachOrder(Order order)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editOrderStateBreach @orderId, @updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@orderId", SqlDbType.Int).Value = order.OrderId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = order.UpdateTime;
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
                    string invoiceNumber = ds.Tables[0].Rows[0].IsNull("f_invoiceNumber") ? string.Empty :
                                ds.Tables[0].Rows[0].Field<string>("f_invoiceNumber");

                    // 若有違約金，需開立違約金發票

                    if (ds.Tables.Count > 1)
                    {
                        DBResponsePrintInvoiceDto response = new DBResponsePrintInvoiceDto
                        {
                            ElectronicInvoiceId = ds.Tables[1].Rows[0].IsNull("f_electronicInvoiceId") ? 0 :
                                ds.Tables[1].Rows[0].Field<int>("f_electronicInvoiceId"),
                            InvoiceNumber = ds.Tables[1].Rows[0].IsNull("f_invoiceNumber") ? string.Empty :
                                ds.Tables[1].Rows[0].Field<string>("f_invoiceNumber"),
                            RandomNumber = ds.Tables[1].Rows[0].IsNull("f_randomNumber") ? string.Empty :
                                ds.Tables[1].Rows[0].Field<string>("f_randomNumber"),
                            TotalAmount = ds.Tables[1].Rows[0].IsNull("f_totalAmount") ? 0 :
                                ds.Tables[1].Rows[0].Field<int>("f_totalAmount"),
                            PlanName = ds.Tables[1].Rows[0].IsNull("f_planName") ? string.Empty :
                                ds.Tables[1].Rows[0].Field<string>("f_planName"),
                        };

                        return (errorCodeNumber, invoiceNumber, response);
                    }

                    return (errorCodeNumber, invoiceNumber, null);
                }

                return (errorCodeNumber, null, null);
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
