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
    public class GroupClassShowcaseRepository : IGroupClassShowcaseRepository
    {
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 新增展示用團課
        /// </summary>
        public ResultWithException AddShowcase(GroupClassShowcase groupClassShowcase)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_addGroupClassShowcase @name, @summary, @detailContent, " +
                    "@imageUrl, @category, @icon, @sort, @errorCode OUTPUT";

                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = groupClassShowcase.Name;
                cmd.Parameters.Add("@summary", SqlDbType.NVarChar).Value = groupClassShowcase.Summary;
                cmd.Parameters.Add("@detailContent", SqlDbType.NVarChar).Value = groupClassShowcase.DetailContent;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = groupClassShowcase.ImageUrl;
                cmd.Parameters.Add("@category", SqlDbType.Int).Value = groupClassShowcase.Category;
                cmd.Parameters.Add("@icon", SqlDbType.Int).Value = groupClassShowcase.Icon;
                cmd.Parameters.Add("@sort", SqlDbType.Int).Value = groupClassShowcase.Sort;
                SqlParameter errorCodeOutput = new SqlParameter("@errorCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(errorCodeOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();
                errorCodeNumber = (int)errorCodeOutput.Value;

                ResultWithException result = new ResultWithException()
                {
                    ErrorCodeNumber = errorCodeNumber,
                    Exception = null
                };
                return result;
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
        /// 取得展示用課程
        /// </summary>
        public (List<GroupClassShowcase> showcases, int totalPage) GetShowcase(RequestGetShowcaseDto getShowcaseDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            int totalPage = 1;
            List<GroupClassShowcase> showcases = new List<GroupClassShowcase>();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getGroupClassShowcase @searchName, @category, " +
                    "@sortOrder, @sortOption, @recordPerPage, @page, @totalPage OUTPUT";

                if (getShowcaseDto.SearchName == null)
                    cmd.Parameters.Add("@searchName", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@searchName", SqlDbType.VarChar).Value = getShowcaseDto.SearchName;

                if (getShowcaseDto.Category == null)
                    cmd.Parameters.Add("@category", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@category", SqlDbType.Int).Value = getShowcaseDto.Category;

                if (getShowcaseDto.SortOption == null)
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sortOption", SqlDbType.VarChar).Value = getShowcaseDto.SortOption;

                cmd.Parameters.Add("@sortOrder", SqlDbType.VarChar).Value = getShowcaseDto.SortOrder;
                cmd.Parameters.Add("@recordPerPage", SqlDbType.Int).Value = getShowcaseDto.RecordPerPage;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getShowcaseDto.Page;
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
                    GroupClassShowcase showcase = new GroupClassShowcase()
                    {
                        GroupClassShowcaseId = dr.IsNull("f_groupClassShowcaseId") ? 0 : dr.Field<int>("f_groupClassShowcaseId"),
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Category = dr.IsNull("f_category") ? 0 : dr.Field<int>("f_category"),
                        Icon = dr.IsNull("f_icon") ? 0 : dr.Field<int>("f_icon"),
                        Sort = dr.IsNull("f_sort") ? 0 : dr.Field<int>("f_sort"),
                    };
                    showcases.Add(showcase);
                }

                return (showcases, totalPage);
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
        /// 取得展示用團課細項
        /// </summary>
        public GroupClassShowcase GetShowcaseDetail(int groupClassShowcaseId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getGroupClassShowcaseDetail @groupClassShowcaseId";

                cmd.Parameters.Add("@groupClassShowcaseId", SqlDbType.Int).Value = groupClassShowcaseId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    GroupClassShowcase showcase = new GroupClassShowcase()
                    {
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Icon = dr.IsNull("f_icon") ? 0 : dr.Field<int>("f_icon"),
                        Category = dr.IsNull("f_category") ? 0 : dr.Field<int>("f_category"),
                        Sort = dr.IsNull("f_sort") ? 0 : dr.Field<int>("f_sort"),
                        Summary = dr.IsNull("f_summary") ? string.Empty : dr.Field<string>("f_summary"),
                        DetailContent = dr.IsNull("f_detailContent") ? string.Empty : dr.Field<string>("f_detailContent"),
                        ImageUrl = dr.IsNull("f_imageUrl") ? string.Empty : dr.Field<string>("f_imageUrl"),
                    };
                    return showcase;
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
        /// 取得修改展示用團課頁面的資料
        /// </summary>
        public GroupClassShowcase GetShowcaseEditDataById(int showcaseId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_healthope_getGroupClassShowcaseEditDataById @groupClassShowcaseId";

                cmd.Parameters.Add("@groupClassShowcaseId", SqlDbType.Int).Value = showcaseId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    GroupClassShowcase showcase = new GroupClassShowcase()
                    {
                        Name = dr.IsNull("f_name") ? string.Empty : dr.Field<string>("f_name"),
                        Icon = dr.IsNull("f_icon") ? 0 : dr.Field<int>("f_icon"),
                        Category = dr.IsNull("f_category") ? 0 : dr.Field<int>("f_category"),
                        Sort = dr.IsNull("f_sort") ? 0 : dr.Field<int>("f_sort"),
                        Summary = dr.IsNull("f_summary") ? string.Empty : dr.Field<string>("f_summary"),
                        DetailContent = dr.IsNull("f_detailContent") ? string.Empty : dr.Field<string>("f_detailContent"),
                        ImageUrl = dr.IsNull("f_imageUrl") ? string.Empty : dr.Field<string>("f_imageUrl"),
                        UpdateTime = dr.IsNull("f_updateTime") ? DateTime.MinValue : dr.Field<DateTime>("f_updateTime")
                    };

                    return showcase;
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
        /// 修改展示用團課
        /// </summary>
        public (ResultWithException result, string oldImageUrl) EditShowcase(RequestEditShowcaseDto editShowcaseDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            int errorCodeNumber;

            try
            {
                cmd.CommandText = "EXEC pro_healthope_editGroupClassShowcase @groupClassShowcaseId, @name, @summary, @detailContent," +
                    "@imageUrl, @category, @icon, @sort, @updateTime, @errorCode OUTPUT";

                if (editShowcaseDto.Name == null)
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = editShowcaseDto.Name;

                if (editShowcaseDto.Summary == null)
                    cmd.Parameters.Add("@summary", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@summary", SqlDbType.NVarChar).Value = editShowcaseDto.Summary;

                if (editShowcaseDto.DetailContent == null)
                    cmd.Parameters.Add("@detailContent", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@detailContent", SqlDbType.NVarChar).Value = editShowcaseDto.DetailContent;

                if (string.IsNullOrEmpty(editShowcaseDto.ImageUrl))
                    cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = editShowcaseDto.ImageUrl;

                if (editShowcaseDto.Category == null)
                    cmd.Parameters.Add("@category", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@category ", SqlDbType.Int).Value = editShowcaseDto.Category;

                if (editShowcaseDto.Icon == null)
                    cmd.Parameters.Add("@icon", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@icon", SqlDbType.Int).Value = editShowcaseDto.Icon;

                if (editShowcaseDto.Sort == null)
                    cmd.Parameters.Add("@sort", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@sort", SqlDbType.Int).Value = editShowcaseDto.Sort;

                cmd.Parameters.Add("@groupClassShowcaseId", SqlDbType.Int).Value = editShowcaseDto.GroupClassShowcaseId;
                cmd.Parameters.Add("@updateTime", SqlDbType.DateTime).Value = editShowcaseDto.UpdateTime;
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

        /// <summary>
        /// 刪除展示用團課
        /// </summary>
        public (bool successFlag, string oldImageUrl) DeleteShowcase(int showcaseId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_healthope_delGroupClassShowcase @groupClassShowcaseId";
                cmd.Parameters.Add("@groupClassShowcaseId", SqlDbType.Int).Value = showcaseId;

                cmd.Connection.Open();

                object imageUrlObj = cmd.ExecuteScalar();
                string imageUrl = string.Empty;

                if (imageUrlObj != null && imageUrlObj != DBNull.Value)
                    imageUrl = imageUrlObj.ToString();

                bool success = imageUrl != null;

                return (success, imageUrl);
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
