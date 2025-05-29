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
    public class TermRepository : ITermRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得舊條款
        /// </summary>
        public List<Term> GetOldTerm(Term getOldTerm)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<Term> terms = new List<Term>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getOldTerm @type, @applicableTarget";


                cmd.Parameters.Add("@type", SqlDbType.TinyInt).Value = getOldTerm.Type;
                cmd.Parameters.Add("@applicableTarget", SqlDbType.TinyInt).Value = getOldTerm.ApplicableTarget;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    Term term = new Term()
                    {
                        TermId = dr.IsNull("f_termId") ? 0 : dr.Field<int>("f_termId"),
                        Version = dr.IsNull("f_version") ? 0 : dr.Field<int>("f_version"),
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        DetailContent = dr.IsNull("f_detailContent") ? string.Empty : dr.Field<string>("f_detailContent"),
                    };
                    terms.Add(term);
                }

                return terms;
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
        /// 新增條款
        /// </summary>
        public bool AddTerm(Term term)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addTerm @detailContent, @type, @applicableTarget, @versionDescription";

                cmd.Parameters.Add("@detailContent", SqlDbType.NVarChar).Value = term.DetailContent;
                cmd.Parameters.Add("@type", SqlDbType.TinyInt).Value = term.Type;
                cmd.Parameters.Add("@applicableTarget", SqlDbType.TinyInt).Value = term.ApplicableTarget;
                cmd.Parameters.Add("@versionDescription", SqlDbType.NVarChar).Value = term.VersionDescription;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt > 0)
                {
                    return true;
                }

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
        /// 取得條款
        /// </summary>
        public (List<Term> terms, int totalPage) GetTerm(RequestGetTermDto getTermDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<Term> terms = new List<Term>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getTerm @type, @status, @applicableTarget, " +
                    "@recordPerPage, @page, @totalPage OUTPUT";

                if (getTermDto.Type == null)
                    cmd.Parameters.Add("@type", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@type", SqlDbType.TinyInt).Value = getTermDto.Type;

                if (getTermDto.Status == null)
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@status", SqlDbType.TinyInt).Value = getTermDto.Status;

                if (getTermDto.ApplicableTarget == null)
                    cmd.Parameters.Add("@applicableTarget", SqlDbType.TinyInt).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@applicableTarget", SqlDbType.TinyInt).Value = getTermDto.ApplicableTarget;

                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getTermDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getTermDto.Page;
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
                    Term term = new Term()
                    {
                        TermId = dr.IsNull("f_termId") ? 0 : dr.Field<int>("f_termId"),
                        Version = dr.IsNull("f_version") ? 0 : dr.Field<int>("f_version"),
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Type = (byte)(dr.IsNull("f_type") ? 0 : dr.Field<byte>("f_type")),
                        ApplicableTarget = (byte)(dr.IsNull("f_applicableTarget") ? 0 : dr.Field<byte>("f_applicableTarget")),
                        Status = (byte)(dr.IsNull("f_status") ? 0 : dr.Field<byte>("f_status")),
                        EffectiveTime = dr.IsNull("f_effectiveTime") ? DateTime.MinValue : dr.Field<DateTime>("f_effectiveTime"),
                    };
                    terms.Add(term);
                }

                return (terms, totalPage);
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
