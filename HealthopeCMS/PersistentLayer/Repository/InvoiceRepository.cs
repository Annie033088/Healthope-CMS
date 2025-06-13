using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增發票字軌
        /// </summary>
        public bool AddInvoiceTrackNumber(InvoiceTrackNumber trackNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addInoviceTrackNumber @trackPrefix, @startNumber, @endNumber, @invoicePeriod";

                cmd.Parameters.Add("@trackPrefix", SqlDbType.Char).Value = trackNumber.TrackPrefix;
                cmd.Parameters.Add("@startNumber", SqlDbType.Int).Value = trackNumber.StartNumber;
                cmd.Parameters.Add("@endNumber", SqlDbType.Int).Value = trackNumber.EndNumber;
                cmd.Parameters.Add("@invoicePeriod", SqlDbType.Int).Value = trackNumber.InvoicePeriod;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt > 0) return true;

                return false;
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
        /// 取得字軌
        /// </summary>
        public (List<InvoiceTrackNumber> invoiceTrackNumbers, int totalPage) GetInvoiceTrackNumber(
            RequestGetInvoiceTrackNumberDto getInvoiceTrackNumberDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage;
            List<InvoiceTrackNumber> invoiceTrackNumbers = new List<InvoiceTrackNumber>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getInvoiceTrackNumber @status, @time, " +
                    "@recordPerPage, @page, @totalPage OUTPUT";

                if (getInvoiceTrackNumberDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = getInvoiceTrackNumberDto.Status;

                if (getInvoiceTrackNumberDto.Time == null)
                    cmd.Parameters.Add("@time", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@time", SqlDbType.Bit).Value = getInvoiceTrackNumberDto.Time;

                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getInvoiceTrackNumberDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getInvoiceTrackNumberDto.Page;
                SqlParameter totalPageOutput = new SqlParameter("@totalPage", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(totalPageOutput);


                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);
                totalPage = (int)totalPageOutput.Value;

                cmd.Connection.Close();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    InvoiceTrackNumber invoiceTrackNumber = new InvoiceTrackNumber()
                    {
                        InvoiceTrackNumberId = dr.IsNull("f_invoiceTrackNumberId") ? 0 : dr.Field<int>("f_invoiceTrackNumberId"),
                        TrackPrefix = dr.IsNull("f_trackPrefix") ? string.Empty : dr.Field<string>("f_trackPrefix"),
                        StartNumber = dr.IsNull("f_startNumber") ? 0 : dr.Field<int>("f_startNumber"),
                        EndNumber = dr.IsNull("f_endNumber") ? 0 : dr.Field<int>("f_endNumber"),
                        CurrentNumber = dr.IsNull("f_currentNumber") ? 0 : dr.Field<int>("f_currentNumber"),
                        InvoicePeriod = dr.IsNull("f_invoicePeriod") ? 0 : dr.Field<int>("f_invoicePeriod"),
                        Status = (byte)(dr.IsNull("f_status") ? 0 : dr.Field<byte>("f_status")),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime"),
                    };
                    invoiceTrackNumbers.Add(invoiceTrackNumber);
                }

                return (invoiceTrackNumbers, totalPage);
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
        /// 修改字軌狀態
        /// </summary>
        public int EditInvoiceTrackNumberStatus(InvoiceTrackNumber invoiceTrackNumber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editInvoiceTrackNumberStatus @invoiceTrackNumberId, @status, " +
                    "@updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@invoiceTrackNumberId", SqlDbType.Int).Value = invoiceTrackNumber.InvoiceTrackNumberId;
                cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = invoiceTrackNumber.Status;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = invoiceTrackNumber.UpdateTime;

                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                errorCodeNumber = (int)errorCodeOutput.Value;

                return errorCodeNumber;
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
        /// 刪除字軌
        /// </summary>
        public bool DeleteInvoiceTrackNumber(int invoiceTrackNumberId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_delInoviceTrackNumber @invoiceTrackNumberId";

                cmd.Parameters.Add("@invoiceTrackNumberId", SqlDbType.Int).Value = invoiceTrackNumberId;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt > 0) return true;

                return false;
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

        public bool EditElectronicInvoiceStatus(bool success, int electronicInvoiceId, string invocieTime)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editElectronicInvoiceStatus @success, @electronicInvoiceId, @invocieTime";

                cmd.Parameters.Add("@success", SqlDbType.Bit).Value = success;
                cmd.Parameters.Add("@electronicInvoiceId", SqlDbType.Int).Value = electronicInvoiceId;

                if (string.IsNullOrEmpty(invocieTime))
                {
                    cmd.Parameters.Add("@invocieTime", SqlDbType.DateTime2).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@invocieTime", SqlDbType.DateTime2).Value = invocieTime;
                }

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt > 0) return true;

                return false;
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
