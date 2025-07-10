using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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
                        UsedSession = dr.IsNull("f_usedSession") ? 0 : dr.Field<int>("f_usedSession"),
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
    }
}
