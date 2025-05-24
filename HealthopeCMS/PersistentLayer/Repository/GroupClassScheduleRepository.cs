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
    public class GroupClassScheduleRepository : IGroupClassScheduleRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得 新增團體課程表前 需要的資料
        /// </summary>
        public (List<GroupClassShowcase> showcases, List<Coach> coaches) GetShowcaseAndCoach(int? category)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            List<GroupClassShowcase> showcases = new List<GroupClassShowcase>();
            List<Coach> coaches = new List<Coach>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getShowcaseAndCoach @category";

                if (category == null)
                    cmd.Parameters.Add("@category", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@category", SqlDbType.Int).Value = category.Value;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Connection.Close();

                if (ds.Tables.Count > 0)
                {
                    // 取得展示課
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        GroupClassShowcase showcase = new GroupClassShowcase();
                        showcase.Name = ds.Tables[0].Rows[i].IsNull("f_name") ?
                            string.Empty : ds.Tables[0].Rows[i].Field<string>("f_name");
                        showcase.Icon = ds.Tables[0].Rows[i].IsNull("f_icon") ?
                            0 : ds.Tables[0].Rows[i].Field<int>("f_icon");
                        showcase.Category = ds.Tables[0].Rows[i].IsNull("f_category") ?
                            0 : ds.Tables[0].Rows[i].Field<int>("f_category");

                        showcases.Add(showcase);
                    }

                    // 取得教練
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        Coach coach = new Coach();
                        coach.CoachId = ds.Tables[1].Rows[i].IsNull("f_coachId") ? 0 :
                            ds.Tables[1].Rows[i].Field<int>("f_coachId");
                        coach.Name = ds.Tables[1].Rows[i].IsNull("f_name") ?
                            string.Empty : ds.Tables[1].Rows[i].Field<string>("f_name");
                        coach.UpdateTime = ds.Tables[1].Rows[i].IsNull("f_updateTime") ?
                            DateTime.MinValue : ds.Tables[1].Rows[i].Field<DateTime>("f_updateTime");

                        coaches.Add(coach);
                    }

                    return (showcases, coaches);
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
        /// 新增團課 schedule
        /// </summary>
        public int AddSchedule(GroupClassSchedule schedule, Coach coach)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addSchedule @className, @category, @icon, @time, @place, " +
                    "@maximumParticipant, @coachId, @coachUpdateTime, @errorCode OUTPUT";

                cmd.Parameters.Add("@className", SqlDbType.NVarChar).Value = schedule.ClassName;
                cmd.Parameters.Add("@category", SqlDbType.Int).Value = schedule.Category;
                cmd.Parameters.Add("@icon", SqlDbType.Int).Value = schedule.Icon;
                cmd.Parameters.Add("@time", SqlDbType.DateTime2).Value = schedule.Time;
                cmd.Parameters.Add("@place", SqlDbType.NVarChar).Value = schedule.Place;
                cmd.Parameters.Add("@maximumParticipant", SqlDbType.TinyInt).Value = schedule.MaximumParticipant;
                cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = coach.CoachId;
                cmd.Parameters.Add("@coachUpdateTime", SqlDbType.DateTime2).Value = coach.UpdateTime;
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
        /// 取得團課 schedule
        /// </summary>
        public (List<GroupClassSchedule> schedules, int totalPage) GetSchedule(RequestGetGroupClassScheduleDto getScheduleDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<Coach> coaches = new List<Coach>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getCoach @searchName, @searchPhone, @status, " +
                    "@sortOrder, @sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getScheduleDto.SearchName == null)
                    cmd.Parameters.Add("@searchName", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@searchName", SqlDbType.VarChar).Value = getScheduleDto.SearchName;

                if (getScheduleDto.SearchPhone == null)
                    cmd.Parameters.Add("@searchPhone", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@searchPhone", SqlDbType.VarChar).Value = getScheduleDto.SearchPhone;

                if (getScheduleDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = getScheduleDto.Status;

                if (getScheduleDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getScheduleDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getScheduleDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getScheduleDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getScheduleDto.Page;
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
                    Coach coach = new Coach()
                    {
                        CoachId = dr.IsNull("f_coachId") ? 0 : dr.Field<int>("f_coachId"),
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Phone = dr.IsNull("f_phone") ? 0 : dr.Field<int>("f_phone"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        Type = (byte)(dr.IsNull("f_type") ? 0 : dr.Field<byte>("f_type")),
                        ContractStartTime = dr.IsNull("f_contractStartTime") ?
                            DateTime.MinValue : dr.Field<DateTime>("f_contractStartTime"),
                        ContractEndTime = dr.IsNull("f_contractEndTime") ?
                            DateTime.MinValue : dr.Field<DateTime>("f_contractEndTime"),
                    };
                    coaches.Add(coach);
                }

                return (coaches, totalPage);
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
