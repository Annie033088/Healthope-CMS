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
    }
}
