using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class MemberClassRepository : IMemberClassRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得新增教練課時的教練課跟教練資料
        /// </summary>
        public List<ResponseGetPersonalTrainingPackageAndCoachDto> GetPersonalTrainingPackageAndCoach(int memberId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ResponseGetPersonalTrainingPackageAndCoachDto> responseList = new List<ResponseGetPersonalTrainingPackageAndCoachDto>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getPersonalTrainingPackageAndCoach @memberId";

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = memberId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    ResponseGetPersonalTrainingPackageAndCoachDto responseGet = new ResponseGetPersonalTrainingPackageAndCoachDto()
                    {
                        MemberPersonalTrainingPackageId = dr.IsNull("f_memberPersonalTrainingPackageId") ?
                            0 : dr.Field<int>("f_memberPersonalTrainingPackageId"),
                        CoachId = dr.IsNull("f_coachId") ? 0 : dr.Field<int>("f_coachId"),
                        CoachPhone = dr.IsNull("f_coachPhone") ? 0 : dr.Field<int>("f_coachPhone"),
                        CoachName = dr.IsNull("f_coachName") ? string.Empty : dr.Field<string>("f_coachName"),
                        PlanName = dr.IsNull("f_planName") ? string.Empty : dr.Field<string>("f_planName"),
                        UsedSession = dr.IsNull("f_usedSessionCount") ? 0 : dr.Field<int>("f_usedSessionCount"),
                        SessionCount = dr.IsNull("f_sessionCount") ? 0 : dr.Field<int>("f_sessionCount"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime"),
                    };

                    responseList.Add(responseGet);
                }

                return responseList;
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
        /// 新增會員預約教練課程
        /// </summary>
        public int AddMemberPersonalClass(MemberPersonalClass memberPersonalClass)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addMemberPersonalClass @memberPersonalTrainingPackageId, @memberId, @coachId, " +
                    "@time, @errorCode OUTPUT";

                cmd.Parameters.Add("@memberPersonalTrainingPackageId", SqlDbType.Int).Value
                    = memberPersonalClass.MemberPersonalTrainingPackageId;
                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = memberPersonalClass.MemberId;
                cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = memberPersonalClass.CoachId;
                cmd.Parameters.Add("@time", SqlDbType.DateTime2).Value = memberPersonalClass.Time;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
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
        /// 取得會員預約的教練課程列表
        /// </summary>
        public ResponseGetMemberPersonalClassListDto GetMemberPersonalClass(RequestGetMemberPersonalClassDto getMemberPersonalClassDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<ResponseGetMemberPersonalClassDto> memberPersonalClasses = new List<ResponseGetMemberPersonalClassDto>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getMemberPersonalClass @searchPhone, @status, @sortOrder, @sortOption," +
                    " @recordPerPage, @page, @totalPage OUTPUT";

                if (getMemberPersonalClassDto.SearchPhone == null)
                    cmd.Parameters.Add("@searchPhone", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@searchPhone", SqlDbType.VarChar).Value = getMemberPersonalClassDto.SearchPhone;

                if (getMemberPersonalClassDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = getMemberPersonalClassDto.Status;

                if (getMemberPersonalClassDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getMemberPersonalClassDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getMemberPersonalClassDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getMemberPersonalClassDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getMemberPersonalClassDto.Page;
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
                    ResponseGetMemberPersonalClassDto memberPersonalClass = new ResponseGetMemberPersonalClassDto()
                    {
                        MemberPersonalClassId = dr.IsNull("f_memberPersonalClassId") ? 0 : dr.Field<int>("f_memberPersonalClassId"),
                        MemberId = dr.IsNull("f_memberId") ? 0 : dr.Field<int>("f_memberId"),
                        MemberName = dr.IsNull("f_memberName") ? string.Empty : dr.Field<string>("f_memberName"),
                        MemberPhone = dr.IsNull("f_memberPhone") ? 0 : dr.Field<int>("f_memberPhone"),
                        CoachId = dr.IsNull("f_coachId") ? 0 : dr.Field<int>("f_coachId"),
                        CoachName = dr.IsNull("f_coachName") ? string.Empty : dr.Field<string>("f_coachName"),
                        Time = dr.IsNull("f_time") ? DateTime.MinValue : dr.Field<DateTime>("f_time"),
                        Category = dr.IsNull("f_category") ? false : dr.Field<bool>("f_category"),
                        Status = (byte)(dr.IsNull("f_status") ? 0 : dr.Field<byte>("f_status")),
                        Remark = dr.IsNull("f_remark") ? string.Empty : dr.Field<string>("f_remark"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };
                    memberPersonalClasses.Add(memberPersonalClass);
                }

                ResponseGetMemberPersonalClassListDto response = new ResponseGetMemberPersonalClassListDto
                {
                    MemberPersonalClassList = memberPersonalClasses,
                    TotalPage = totalPage,
                };

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
        /// 修改預約課程備註
        /// </summary>
        public bool EditMemberPersonalClassRemark(MemberPersonalClass memberPersonalClass)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editMemberPersonalClassRemark @memberPersonalClassId, @remark, @updateTime";

                cmd.Parameters.Add("@memberPersonalClassId", SqlDbType.Int).Value = memberPersonalClass.MemberPersonalClassId;
                cmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = memberPersonalClass.Remark;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = memberPersonalClass.UpdateTime;

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
        /// 修改會員的教練預約課程狀態
        /// </summary>
        public int EditMemberPersonalClassStatus(MemberPersonalClass memberPersonalClass)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editMemberPersonalClassStatus @memberPersonalClassId," +
                    " @status, @updateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@memberPersonalClassId", SqlDbType.Int).Value
                    = memberPersonalClass.MemberPersonalClassId;
                cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = memberPersonalClass.Status;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = memberPersonalClass.UpdateTime;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
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
        /// 每日取消當日預約中的教練課程 (預約中課程於一天之前 無確認，即改為取消)
        /// </summary>
        public Task AutoCancelReservingMemberPersonalClass()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editReservingMemberPersonalClassStatusCancel";

                cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                return Task.CompletedTask;
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
