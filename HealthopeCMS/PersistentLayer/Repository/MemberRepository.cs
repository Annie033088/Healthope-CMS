using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ApiLayer.Models.Member.Request;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace PersistentLayer.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得會員列表
        /// </summary>
        public (List<Member> members, int totalPage) GetMember(RequestGetMemberDto getMemberDto)

        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<Member> members = new List<Member>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getMember @searchName, @searchPhone, @status, @sortOrder, @sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getMemberDto.SearchName == null)
                    cmd.Parameters.Add("@searchName", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@searchName", SqlDbType.VarChar).Value = getMemberDto.SearchName;

                if (getMemberDto.SearchPhone == null)
                    cmd.Parameters.Add("@searchPhone", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@searchPhone", SqlDbType.VarChar).Value = getMemberDto.SearchPhone;

                if (getMemberDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = getMemberDto.Status;

                if (getMemberDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getMemberDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getMemberDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getMemberDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getMemberDto.Page;
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
                    Member member = new Member()
                    {
                        MemberId = dr.IsNull("f_memberId") ? 0 : dr.Field<int>("f_memberId"),
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Phone = dr.IsNull("f_phone") ? 0 : dr.Field<int>("f_phone"),
                        PhoneVerified = dr.IsNull("f_phoneVerified") ? false : dr.Field<bool>("f_phoneVerified"),
                        Email = dr.IsNull("f_email") ? string.Empty : dr.Field<string>("f_email"),
                        MembershipExpiry = dr.IsNull("f_membershipExpiry") ? DateTime.MinValue : dr.Field<DateTime>("f_membershipExpiry"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        AbsenceTime = (byte)(dr.IsNull("f_absenceTime") ? 0 : dr.Field<byte>("f_absenceTime")),
                        AllowGroupClass = dr.IsNull("f_allowGroupClass") ? DateTime.MinValue : dr.Field<DateTime>("f_allowGroupClass")
                    };
                    members.Add(member);
                }

                return (members, totalPage);
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
        /// 根據 id 取得修改會員時需要的資料
        /// </summary>
        public Member GetMemberEditDataById(int memberId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getMemberEditDataById @memberId";

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = memberId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    Member member = new Member()
                    {
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Phone = dr.IsNull("f_phone") ? 0 : dr.Field<int>("f_phone"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };

                    return member;
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
        /// 修改會員手機或狀態
        /// </summary>
        public int EditMember(RequestEditMemberDto editMemberDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editMember @memberId, @status, @phone, @updateTime, " +
                    "@errorCode OUTPUT";

                if (editMemberDto.Status == null)
                {
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = editMemberDto.Status;
                }

                if (editMemberDto.Phone == null)
                {
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = editMemberDto.Phone;
                }

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = editMemberDto.MemberId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime2).Value = editMemberDto.UpdateTime;
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
        /// 取得會員詳細資料
        /// </summary>
        public (Member member, List<MemberMembershipPlan> memberMembershipPlans,
            List<MemberPersonalTrainingPackage> memberPersonalTrainingPackages, List<Coach> coaches) GetMemberDetail(int memberId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            List<MemberMembershipPlan> memberMembershipPlans = new List<MemberMembershipPlan>();
            List<MemberPersonalTrainingPackage> memberPersonalTrainingPackages = new List<MemberPersonalTrainingPackage>();
            List<Coach> coaches = new List<Coach>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getMemberDetail @memberId";

                cmd.Parameters.Add("@memberId", SqlDbType.Int).Value = memberId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Connection.Close();

                if (ds.Tables.Count > 0)
                {
                    DataRow memberDataRow = ds.Tables[0].Rows[0];
                    Member member = new Member()
                    {
                        Name = memberDataRow.IsNull("f_name") ? string.Empty : memberDataRow.Field<string>("f_name"),
                        Phone = memberDataRow.IsNull("f_phone") ? 0 : memberDataRow.Field<int>("f_phone"),
                        Email = memberDataRow.IsNull("f_email") ? string.Empty : memberDataRow.Field<string>("f_email"),
                        PhoneVerified = memberDataRow.IsNull("f_phoneVerified") ? false : memberDataRow.Field<bool>("f_phoneVerified"),
                        PhotoUrl = memberDataRow.IsNull("f_photoUrl") ? string.Empty : memberDataRow.Field<string>("f_photoUrl"),
                        Gender = (byte)(memberDataRow.IsNull("f_gender") ? 0 : memberDataRow.Field<byte>("f_gender")),
                        Height = memberDataRow.IsNull("f_height") ? 0 : memberDataRow.Field<int>("f_height"),
                        Weight = memberDataRow.IsNull("f_weight") ? 0 : memberDataRow.Field<int>("f_weight"),
                        Status = memberDataRow.IsNull("f_status") ? false : memberDataRow.Field<bool>("f_status"),
                        AbsenceTime = (byte)(memberDataRow.IsNull("f_gender") ? 0 : memberDataRow.Field<byte>("f_gender")),
                        EmergencyContactName = memberDataRow.IsNull("f_emergencyContactName") ?
                            string.Empty : memberDataRow.Field<string>("f_emergencyContactName"),
                        EmergencyContactPhone = memberDataRow.IsNull("f_emergencyContactPhone") ?
                            0 : memberDataRow.Field<int>("f_emergencyContactPhone"),
                        EmergencyContactRelation = memberDataRow.IsNull("f_emergencyContactRelation") ?
                            string.Empty : memberDataRow.Field<string>("f_emergencyContactRelation"),
                        Birthday = memberDataRow.IsNull("f_birthday") ? DateTime.MinValue
                            : memberDataRow.Field<DateTime>("f_birthday"),
                        AllowGroupClass = memberDataRow.IsNull("f_allowGroupClass") ? DateTime.MinValue
                            : memberDataRow.Field<DateTime>("f_allowGroupClass"),
                        MembershipExpiry = memberDataRow.IsNull("f_membershipExpiry") ? DateTime.MinValue
                            : memberDataRow.Field<DateTime>("f_membershipExpiry"),
                        CreateTime = memberDataRow.IsNull("f_createTime") ? DateTime.MinValue
                            : memberDataRow.Field<DateTime>("f_createTime"),
                    };

                    // 取得會籍方案
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        MemberMembershipPlan memberMembershipPlan = new MemberMembershipPlan();
                        memberMembershipPlan.MemberMembershipPlanId =
                            ds.Tables[1].Rows[i].IsNull("f_memberMembershipPlanId") ? 0 :
                            ds.Tables[1].Rows[i].Field<int>("f_memberMembershipPlanId");
                        memberMembershipPlan.PlanName = ds.Tables[1].Rows[i].IsNull("f_planName") ?
                            string.Empty : ds.Tables[1].Rows[i].Field<string>("f_planName");
                        memberMembershipPlan.Duration = (byte)(ds.Tables[1].Rows[i].IsNull("f_duration") ? 0 :
                            ds.Tables[1].Rows[i].Field<byte>("f_duration"));
                        memberMembershipPlan.Status = (byte)(ds.Tables[1].Rows[i].IsNull("f_status") ? 0 :
                            ds.Tables[1].Rows[i].Field<byte>("f_status"));
                        memberMembershipPlan.EndDate = ds.Tables[1].Rows[i].IsNull("f_endDate") ?
                            DateTime.MinValue : ds.Tables[1].Rows[i].Field<DateTime>("f_endDate");
                        memberMembershipPlan.UpdateTime = ds.Tables[1].Rows[i].IsNull("f_updateTime") ?
                            DateTime.MinValue : ds.Tables[1].Rows[i].Field<DateTime>("f_updateTime");

                        memberMembershipPlans.Add(memberMembershipPlan);
                    }

                    // 取得教練課方案
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        MemberPersonalTrainingPackage memberPTPackage = new MemberPersonalTrainingPackage();
                        memberPTPackage.MemberPersonalTrainingPackageId =
                            ds.Tables[2].Rows[i].IsNull("f_memberPersonalTrainingPackageId") ? 0 :
                            ds.Tables[2].Rows[i].Field<int>("f_memberPersonalTrainingPackageId");
                        memberPTPackage.CoachId =
                            ds.Tables[2].Rows[i].IsNull("f_coachId") ? 0 :
                            ds.Tables[2].Rows[i].Field<int>("f_coachId");
                        memberPTPackage.PlanName = ds.Tables[2].Rows[i].IsNull("f_planName") ?
                            string.Empty : ds.Tables[2].Rows[i].Field<string>("f_planName");
                        memberPTPackage.UsedSessionCount = ds.Tables[2].Rows[i].IsNull("f_usedSessionCount") ?
                            0 : ds.Tables[2].Rows[i].Field<int>("f_usedSessionCount");
                        memberPTPackage.SessionCount =
                            ds.Tables[2].Rows[i].IsNull("f_sessionCount") ? 0 :
                            ds.Tables[2].Rows[i].Field<int>("f_sessionCount");
                        memberPTPackage.Status = (byte)(ds.Tables[2].Rows[i].IsNull("f_status") ? 0 :
                            ds.Tables[2].Rows[i].Field<byte>("f_status"));
                        memberPTPackage.UpdateTime = ds.Tables[2].Rows[i].IsNull("f_updateTime") ?
                            DateTime.MinValue : ds.Tables[2].Rows[i].Field<DateTime>("f_updateTime");

                        memberPersonalTrainingPackages.Add(memberPTPackage);
                    }

                    // 取得教練
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        Coach coach = new Coach();
                        coach.CoachId =
                            ds.Tables[3].Rows[i].IsNull("f_coachId") ? 0 :
                            ds.Tables[3].Rows[i].Field<int>("f_coachId");
                        coach.Phone =
                            ds.Tables[3].Rows[i].IsNull("f_phone") ? 0 :
                            ds.Tables[3].Rows[i].Field<int>("f_phone");
                        coach.Name = ds.Tables[3].Rows[i].IsNull("f_name") ?
                            string.Empty : ds.Tables[3].Rows[i].Field<string>("f_name");
                        coach.UpdateTime = ds.Tables[3].Rows[i].IsNull("f_updateTime") ?
                            DateTime.MinValue : ds.Tables[3].Rows[i].Field<DateTime>("f_updateTime");

                        coaches.Add(coach);
                    }

                    return (member, memberMembershipPlans, memberPersonalTrainingPackages, coaches);
                }

                return (null, null, null, null);
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
        /// 根據電話或名稱取得會員
        /// </summary>
        public List<Member> GetMemberByNameOrPhone(RequestGetMemberByNameOrPhoneDto getMemberDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<Member> members = new List<Member>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getMemberByNameOrPhone @name, @phone";

                if (getMemberDto.Name == null)
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = getMemberDto.Name;

                if (getMemberDto.Phone == null)
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@phone", SqlDbType.Int).Value = getMemberDto.Phone;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    Member member = new Member()
                    {
                        MemberId = dr.IsNull("f_memberId") ? 0 : dr.Field<int>("f_memberId"),
                        Phone = dr.IsNull("f_phone") ? 0 : dr.Field<int>("f_phone"),
                        PhoneVerified = dr.IsNull("f_phoneVerified") ? false : dr.Field<bool>("f_phoneVerified"),
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                    };
                    members.Add(member);
                }

                return (members);
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
