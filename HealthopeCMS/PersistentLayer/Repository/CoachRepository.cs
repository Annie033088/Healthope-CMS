using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using System.Configuration;

namespace PersistentLayer.Repository
{
    public class CoachRepository : ICoachRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增教練
        /// </summary>
        public OperationResult AddCoach(Coach coach)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addCoach @name, @phone, @email, @type, " +
                    "@contractStartTime, @contractEndTime, @account, @hash, @introduction, @specialty, " +
                    "@certification, @photoUrl, @errorCode OUTPUT";

                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = coach.Name;
                cmd.Parameters.Add("@phone", SqlDbType.Int).Value = coach.Phone;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = coach.Email;
                cmd.Parameters.Add("@type", SqlDbType.TinyInt).Value = coach.Type;
                cmd.Parameters.Add("@contractStartTime", SqlDbType.Date).Value = coach.ContractStartTime;
                cmd.Parameters.Add("@contractEndTime", SqlDbType.Date).Value = coach.ContractEndTime;
                cmd.Parameters.Add("@account", SqlDbType.VarChar).Value = coach.Account;
                cmd.Parameters.Add("@hash", SqlDbType.VarChar).Value = coach.Hash;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = coach.Introduction;
                cmd.Parameters.Add("@specialty", SqlDbType.NVarChar).Value = coach.Specialty;
                cmd.Parameters.Add("@certification", SqlDbType.NVarChar).Value = coach.Certification;
                cmd.Parameters.Add("@photoUrl", SqlDbType.NVarChar).Value = coach.PhotoUrl;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
                errorCodeNumber = (int)errorCodeOutput.Value;

                OperationResult result = new OperationResult()
                {
                    ErrorCodeNumber = errorCodeNumber,
                    Exception = null
                };
                return result;
            }
            catch (Exception ex)
            {
                int serverError = 6;
                OperationResult result = new OperationResult()
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
        /// 取得教練清單
        /// </summary>
        public (List<Coach> coaches, int totalPage) GetCoach(RequestGetCoachDto getCoachDto)
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

                if (getCoachDto.SearchName == null)
                    cmd.Parameters.Add("@searchName", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@searchName", SqlDbType.VarChar).Value = getCoachDto.SearchName;

                if (getCoachDto.SearchPhone == null)
                    cmd.Parameters.Add("@searchPhone", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@searchPhone", SqlDbType.VarChar).Value = getCoachDto.SearchPhone;

                if (getCoachDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.Bit).Value = getCoachDto.Status;

                if (getCoachDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getCoachDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getCoachDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getCoachDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getCoachDto.Page;
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

        /// <summary>
        /// 取得修改教練頁面的資料
        /// </summary>
        public Coach GetCoachEditDataById(int coachId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getCoachEditDataById @coachId";

                cmd.Parameters.Add("@coachId", SqlDbType.Int).Value = coachId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    Coach coach = new Coach()
                    {
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Phone = dr.IsNull("f_phone") ? 0 : dr.Field<int>("f_phone"),
                        Status = dr.IsNull("f_status") ? false : dr.Field<bool>("f_status"),
                        Email = dr.IsNull("f_email") ? string.Empty : dr.Field<string>("f_email"),
                        ContractStartTime = dr.IsNull("f_contractStartTime") ?
                            DateTime.MinValue : dr.Field<DateTime>("f_contractStartTime"),
                        ContractEndTime = dr.IsNull("f_contractEndTime") ?
                            DateTime.MinValue : dr.Field<DateTime>("f_contractEndTime"),
                        Introduction =dr.IsNull("f_introduction") ? string.Empty : dr.Field<string>("f_introduction"),
                        Specialty = dr.IsNull("f_specialty") ? string.Empty : dr.Field<string>("f_specialty"),
                        Certification = dr.IsNull("f_certification") ? 
                            string.Empty : dr.Field<string>("f_certification"),
                        PhotoUrl = dr.IsNull("f_photoUrl") ? string.Empty : dr.Field<string>("f_photoUrl"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };

                    return coach;
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
