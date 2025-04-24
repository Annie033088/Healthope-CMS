using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DomainLayer.Models;
using PersistentLayer.Interface;

namespace PersistentLayer.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得登入中的管理員資料
        /// </summary>
        public Admin GetLoggingInAdmin(string account)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getLoggingInAdmin @account";

                cmd.Parameters.Add("@account", SqlDbType.VarChar).Value = account;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    Admin admin = new Admin()
                    {
                        AdminId = dr.IsNull("f_adminId") ? 0 : dr.Field<int>("f_adminId"),
                        Hash = dr.IsNull("f_hash") ? string.Empty : dr.Field<string>("f_hash"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        Identity = (byte)(dr.IsNull("f_identity") ? 0 : dr.Field<byte>("f_identity"))
                    };

                    return admin;
                }

                return (null);
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
        /// 新增管理員
        /// </summary>
        public bool AddAdmin(Admin admin)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addAdmin @account, @hash, @positionDescription, @identity";

                cmd.Parameters.Add("@account", SqlDbType.VarChar).Value = admin.Account;
                cmd.Parameters.Add("@hash", SqlDbType.VarChar).Value = admin.Hash;
                cmd.Parameters.Add("@positionDescription", SqlDbType.NVarChar).Value = admin.PositionDescription;
                cmd.Parameters.Add("@identity", SqlDbType.TinyInt).Value = admin.Identity;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                //受影響筆數為1代表成功
                if (ExeCnt == 1)
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
    }
}
