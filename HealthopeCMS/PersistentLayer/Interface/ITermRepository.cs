using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface ITermRepository
    {
        /// <summary>
        /// 取得舊條款
        /// </summary>
        List<Term> GetOldTerm(Term getOldTerm);

        /// <summary>
        /// 新增條款
        /// </summary>
        bool AddTerm(Term term);

        /// <summary>
        /// 取得條款
        /// </summary>
        (List<Term> terms, int totalPage) GetTerm(RequestGetTermDto getTermDto);

        /// <summary>
        /// 取得修改條款頁面的資料
        /// </summary>
        Term GetTermEditDataById(int termId);

        /// <summary>
        /// 修改條款
        /// </summary>
        int EditTerm(RequestEditTermDto editTermDto);

        /// <summary>
        /// 修改條款狀態 (僅限草稿=>發布)
        /// </summary>
        int EditTermStatus(Term editTermStatus);

        /// <summary>
        /// 取得條款的詳細資料
        /// </summary>
        Term GetTermDetail(int termId);

        /// <summary>
        /// 刪除條款
        /// </summary>
        bool DeleteTerm(int termId);
    }
}
