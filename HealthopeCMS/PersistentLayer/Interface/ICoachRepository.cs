using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface ICoachRepository
    {
        /// <summary>
        /// 新增教練
        /// </summary>
        OperationResult addCoach(Coach coach);
    }
}
