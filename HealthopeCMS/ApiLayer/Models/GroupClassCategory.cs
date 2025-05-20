using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiLayer.Models
{
    public enum GroupClassCategory
    {
        None,

        /// <summary>
        /// 有氧
        /// </summary>
        Cardio,

        /// <summary>
        /// 肌力
        /// </summary>
        Force,

        /// <summary>
        /// 瑜伽
        /// </summary>
        Yoga,

        /// <summary>
        /// 舞蹈
        /// </summary>
        Dance,

        /// <summary>
        /// 飛輪
        /// </summary>
        Flywheel,

        /// <summary>
        /// 基礎
        /// </summary>
        Basic,

        /// <summary>
        /// 其他
        /// </summary>
        Other,
    }
}