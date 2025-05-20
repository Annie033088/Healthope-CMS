using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
