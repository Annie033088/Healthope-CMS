using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface IGroupClassShowcaseRepository
    {
        /// <summary>
        /// 新增展示用團課
        /// </summary>
        ResultWithException AddShowcase(GroupClassShowcase groupClassShowcase);

        /// <summary>
        /// 取得展示用課程
        /// </summary>
        (List<GroupClassShowcase> showcases, int totalPage) GetShowcase(RequestGetShowcaseDto getShowcaseDto);

        /// <summary>
        /// 取得展示用課程細項
        /// </summary>
        GroupClassShowcase GetShowcaseDetail(int groupClassShowcaseId);

        /// <summary>
        /// 取得修改展示用團課頁面的資料
        /// </summary>
        GroupClassShowcase GetShowcaseEditDataById(int showcaseId);

        /// <summary>
        /// 修改展示用團課
        /// </summary>
        (ResultWithException result, string oldImageUrl) EditShowcase(RequestEditShowcaseDto editShowcaseDto);

        /// <summary>
        /// 刪除展示用團課
        /// </summary>
        (bool successFlag, string oldImageUrl) DeleteShowcase(int showcaseId);
    }
}
