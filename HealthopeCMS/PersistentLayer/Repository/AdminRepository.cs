using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using ApiLayer.Models.Admin.RequestAdminDto;
using DomainLayer.Models;
using PersistentLayer.Interface;

namespace PersistentLayer.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得正要登入的管理員資料
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
                cmd.CommandText = "EXEC pro_healthope_addAdmin @account, @hash, @identity";

                cmd.Parameters.Add("@account", SqlDbType.VarChar).Value = admin.Account;
                cmd.Parameters.Add("@hash", SqlDbType.VarChar).Value = admin.Hash;
                cmd.Parameters.Add("@identity", SqlDbType.TinyInt).Value = admin.Identity;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                // 受影響筆數為1代表成功
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

        public (List<Admin> admins, int totalPage) GetAdmin(RequestGetAdminDto getAdminDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<Admin> admins = new List<Admin>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getAdmin @searchAccount, @status, @sortOrder, @sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getAdminDto.SearchAccount == null)
                {
                    cmd.Parameters.Add("@searchAccount", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@searchAccount", SqlDbType.VarChar).Value = getAdminDto.SearchAccount;
                }

                if (getAdminDto.Status == null)
                {
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = getAdminDto.Status;
                }

                if (getAdminDto.SortOption == null)
                {
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getAdminDto.SortOption;
                }

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getAdminDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getAdminDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getAdminDto.Page;
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
                    Admin admin = new Admin()
                    {
                        AdminId = dr.IsNull("f_adminId") ? 0 : dr.Field<int>("f_adminId"),
                        Account = dr.IsNull("f_account") ? string.Empty : dr.Field<string>("f_account"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        Identity = (byte)(dr.IsNull("f_identity") ? 0 : dr.Field<byte>("f_identity")),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };
                    admins.Add(admin);
                }

                return (admins, totalPage);
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
        /// 修改管理者
        /// </summary>
        public bool EditAdmin(RequestEditAdminDto editAdminDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editAdmin @adminId, @status, @identity";

                if (editAdminDto.Status == null)
                {
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = editAdminDto.Status;
                }
                if (editAdminDto.Identity == null)
                {
                    cmd.Parameters.Add("@identity", SqlDbType.TinyInt).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@identity", SqlDbType.TinyInt).Value = editAdminDto.Identity;
                }

                cmd.Parameters.Add("@adminId", SqlDbType.Int).Value = editAdminDto.AdminId;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                // 受影響筆數為1代表成功
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
        
        public Admin GetAdminById(int adminId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getAdminById @adminId";

                cmd.Parameters.Add("@adminId", SqlDbType.Int).Value = adminId;

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
                        Account = dr.IsNull("f_account") ? string.Empty : dr.Field<string>("f_account"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        Identity = (byte)(dr.IsNull("f_identity") ? 0 : dr.Field<byte>("f_identity")),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
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
    }
}
