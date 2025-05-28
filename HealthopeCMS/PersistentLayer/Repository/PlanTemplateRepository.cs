using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ApiLayer.Models.PlanTemplate.Request;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class PlanTemplateRepository : IPlanTemplateRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增 一次性票劵方案
        /// </summary>
        public bool AddTicketPlan(TicketPlan ticketPlan)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addTicketPlan @price, @status";

                cmd.Parameters.Add("@price", SqlDbType.Int).Value = ticketPlan.Price;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = ticketPlan.Status;

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
        /// 新增 會籍方案
        /// </summary>
        public ResultWithException AddMembershipPlan(MembershipPlan membershipPlan)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addMembershipPlan @name, @price, @duration, @introduction, " +
                    "@imageUrl, @display, @status";

                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = membershipPlan.Name;
                cmd.Parameters.Add("@price", SqlDbType.Int).Value = membershipPlan.Price;
                cmd.Parameters.Add("@duration", SqlDbType.TinyInt).Value = membershipPlan.Duration;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = membershipPlan.Introduction;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = membershipPlan.ImageUrl;
                cmd.Parameters.Add("@display", SqlDbType.Bit).Value = membershipPlan.Display;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = membershipPlan.Status;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                int success = 1;
                int createFail = 10;

                if (ExeCnt > 0)
                {
                    return new ResultWithException()
                    {
                        ErrorCodeNumber = success,
                        Exception = null,
                    };
                }

                return new ResultWithException()
                {
                    ErrorCodeNumber = createFail,
                    Exception = null,
                };
            }
            catch (Exception ex)
            {
                int serverError = 6;
                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = serverError,
                    Exception = ex
                };
                return result;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 新增 教練課方案
        /// </summary>
        public ResultWithException AddPersonalTrainingPackage(PersonalTrainingPackage personalTrainingPackage)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addPersonalTrainingPackage @name, @price, @sessionCount, @introduction, " +
                    "@imageUrl, @display, @status";

                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = personalTrainingPackage.Name;
                cmd.Parameters.Add("@price", SqlDbType.Int).Value = personalTrainingPackage.Price;
                cmd.Parameters.Add("@sessionCount", SqlDbType.TinyInt).Value = personalTrainingPackage.SessionCount;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = personalTrainingPackage.Introduction;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = personalTrainingPackage.ImageUrl;
                cmd.Parameters.Add("@display", SqlDbType.Bit).Value = personalTrainingPackage.Display;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = personalTrainingPackage.Status;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                int success = 1;
                int createFail = 10;

                if (ExeCnt > 0)
                {
                    return new ResultWithException()
                    {
                        ErrorCodeNumber = success,
                        Exception = null,
                    };
                }

                return new ResultWithException()
                {
                    ErrorCodeNumber = createFail,
                    Exception = null,
                };
            }
            catch (Exception ex)
            {
                int serverError = 6;
                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = serverError,
                    Exception = ex
                };
                return result;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 取得會籍方案
        /// </summary>
        public (List<MembershipPlan> membershipPlans, int totalPage) GetMembershipPlan(RequestGetPlanDto getPlanDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<MembershipPlan> membershipPlans = new List<MembershipPlan>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getMembershipPlan @status, @sortOrder, " +
                    "@sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getPlanDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = getPlanDto.Status;

                if (getPlanDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getPlanDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getPlanDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getPlanDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getPlanDto.Page;
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
                    MembershipPlan membershipPlan = new MembershipPlan()
                    {
                        MembershipPlanId = dr.IsNull("f_membershipPlanId") ? 0 : dr.Field<int>("f_membershipPlanId"),
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Price = dr.IsNull("f_price") ? 0 : dr.Field<int>("f_price"),
                        Display = dr.IsNull("f_display") ? false : dr.Field<bool>("f_display"),
                        Introduction = dr.IsNull("f_introduction") ? string.Empty : dr.Field<string>("f_introduction"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        Duration = (byte)(dr.IsNull("f_duration") ? 0 : dr.Field<byte>("f_duration")),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };
                    membershipPlans.Add(membershipPlan);
                }

                return (membershipPlans, totalPage);
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
        /// 取得教練課方案
        /// </summary>
        public (List<PersonalTrainingPackage> personalTrainingPackages, int totalpage)
            GetPersionalTrainingPackage(RequestGetPlanDto getPlanDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<PersonalTrainingPackage> personalTrainingPackages = new List<PersonalTrainingPackage>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getPersonalTrainingPackage @status, @sortOrder, " +
                    "@sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getPlanDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = getPlanDto.Status;

                if (getPlanDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getPlanDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getPlanDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getPlanDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getPlanDto.Page;
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
                    PersonalTrainingPackage personalTrainingPackage = new PersonalTrainingPackage()
                    {
                        PersonalTrainingPackageId = dr.IsNull("f_personalTrainingPackageId") ?
                            0 : dr.Field<int>("f_personalTrainingPackageId"),
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Price = dr.IsNull("f_price") ? 0 : dr.Field<int>("f_price"),
                        Display = dr.IsNull("f_display") ? false : dr.Field<bool>("f_display"),
                        Introduction = dr.IsNull("f_introduction") ? string.Empty : dr.Field<string>("f_introduction"),
                        SessionCount = dr.IsNull("f_sessionCount") ? 0 : dr.Field<int>("f_sessionCount"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };
                    personalTrainingPackages.Add(personalTrainingPackage);
                }

                return (personalTrainingPackages, totalPage);
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
        /// 取得票劵方案
        /// </summary>
        public (List<TicketPlan> ticketPlans, int totalPage) GetTicketPlan(RequestGetPlanDto getPlanDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<TicketPlan> ticketPlans = new List<TicketPlan>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getTicketPlan @status, @sortOrder, " +
                    "@sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getPlanDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = getPlanDto.Status;

                if (getPlanDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getPlanDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getPlanDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getPlanDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getPlanDto.Page;
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
                    TicketPlan ticketPlan = new TicketPlan()
                    {
                        TicketPlanId = dr.IsNull("f_ticketPlanId") ? 0 : dr.Field<int>("f_ticketPlanId"),
                        Price = dr.IsNull("f_price") ? 0 : dr.Field<int>("f_price"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };
                    ticketPlans.Add(ticketPlan);
                }

                return (ticketPlans, totalPage);
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
        /// 修改票劵方案狀態
        /// </summary>
        public bool EditTicketPlanStatus(TicketPlan ticketPlan)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editTicketPlanStatus @ticketPlanId, @status, @updateTime";

                cmd.Parameters.Add("@ticketPlanId", SqlDbType.Int).Value = ticketPlan.TicketPlanId;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = ticketPlan.Status;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = ticketPlan.UpdateTime;

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

        /// <summary>
        /// 取得修改會籍方案頁面資料
        /// </summary>
        public MembershipPlan GetMembershipPlanEditDataById(int memebershipPlanId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getMembershipPlanEditDataById @memebershipPlanId";

                cmd.Parameters.Add("@memebershipPlanId", SqlDbType.Int).Value = memebershipPlanId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    MembershipPlan membershipPlan = new MembershipPlan()
                    {
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        Display = dr.IsNull("f_display") ? false : dr.Field<bool>("f_display"),
                        Introduction = dr.IsNull("f_introduction") ? string.Empty : dr.Field<string>("f_introduction"),
                        ImageUrl = dr.IsNull("f_imageUrl") ? string.Empty : dr.Field<string>("f_imageUrl"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };

                    return membershipPlan;
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
        /// 取得修改教練課方案頁面資料
        /// </summary>
        public PersonalTrainingPackage GetPersonalTrainingPackageEditDataById(int personalTrainingPackageId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getPersonalTrainingPackageEditDataById @personalTrainingPackageId";

                cmd.Parameters.Add("@personalTrainingPackageId", SqlDbType.Int).Value = personalTrainingPackageId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    PersonalTrainingPackage personalTrainingPackage = new PersonalTrainingPackage()
                    {
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        Display = dr.IsNull("f_display") ? false : dr.Field<bool>("f_display"),
                        Introduction = dr.IsNull("f_introduction") ? string.Empty : dr.Field<string>("f_introduction"),
                        ImageUrl = dr.IsNull("f_imageUrl") ? string.Empty : dr.Field<string>("f_imageUrl"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };

                    return personalTrainingPackage;
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
        /// 修改會籍方案
        /// </summary>
        public (ResultWithException result, string oldImageUrl) EditMembershipPlan(RequestEditMembershipPlanDto editMembershipPlanDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editMembershipPlan @membershipPlanId, @introduction, @imageUrl, " +
                    "@status, @display, @updateTime, @errorCode OUTPUT";

                if (editMembershipPlanDto.Introduction == null)
                    cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = editMembershipPlanDto.Introduction;

                if (string.IsNullOrEmpty(editMembershipPlanDto.ImageUrl))
                    cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = editMembershipPlanDto.ImageUrl;

                if (editMembershipPlanDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status ", SqlDbType.Bit).Value = editMembershipPlanDto.Status;

                if (editMembershipPlanDto.Display == null)
                    cmd.Parameters.Add("@display", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@display ", SqlDbType.Bit).Value = editMembershipPlanDto.Display;

                cmd.Parameters.Add("@membershipPlanId", SqlDbType.Int).Value = editMembershipPlanDto.MembershipPlanId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = editMembershipPlanDto.UpdateTime;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                object imageUrlObj = cmd.ExecuteScalar();
                string imageUrl = string.Empty;

                if (imageUrlObj != null && imageUrlObj != DBNull.Value)
                    imageUrl = imageUrlObj.ToString();

                errorCodeNumber = (int)errorCodeOutput.Value;

                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = errorCodeNumber,
                    Exception = null
                };
                return (result, imageUrl);
            }
            catch (Exception ex)
            {
                int serverError = 6;
                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = serverError,
                    Exception = ex
                };
                return (result, string.Empty);
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }

        public (ResultWithException result, string oldImageUrl) EditPersonalTrainingPackage(
            RequestEditPersonalTrainingPackageDto editPlanDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editPersonalTrainingPackage @personalTrainingPackageId, @introduction, " +
                    "@imageUrl, @status, @display, @updateTime, @errorCode OUTPUT";

                if (editPlanDto.Introduction == null)
                    cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = editPlanDto.Introduction;

                if (string.IsNullOrEmpty(editPlanDto.ImageUrl))
                    cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = editPlanDto.ImageUrl;

                if (editPlanDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status ", SqlDbType.Bit).Value = editPlanDto.Status;

                if (editPlanDto.Display == null)
                    cmd.Parameters.Add("@display", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@display ", SqlDbType.Bit).Value = editPlanDto.Display;

                cmd.Parameters.Add("@personalTrainingPackageId", SqlDbType.Int).Value = editPlanDto.PersonalTrainingPackageId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = editPlanDto.UpdateTime;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                object imageUrlObj = cmd.ExecuteScalar();
                string imageUrl = string.Empty;

                if (imageUrlObj != null && imageUrlObj != DBNull.Value)
                    imageUrl = imageUrlObj.ToString();

                errorCodeNumber = (int)errorCodeOutput.Value;

                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = errorCodeNumber,
                    Exception = null
                };
                return (result, imageUrl);
            }
            catch (Exception ex)
            {
                int serverError = 6;
                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = serverError,
                    Exception = ex
                };
                return (result, string.Empty);
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }
    }
}
