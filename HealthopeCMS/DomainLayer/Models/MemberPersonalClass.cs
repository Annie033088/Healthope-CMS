using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class MemberPersonalClass
    {
        /// <summary>
        /// 會員預約的私人課 Id
        /// </summary>
        public int MemberPersonalClassId { get; set; }

        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 教練 Id
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 會員的教練課方案 Id
        /// </summary>
        public int MemberPersonalTrainingPackageId { get; set; }

        /// <summary>
        /// 上課時間
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 課程分類 0:體驗課程 ; 1:付費課程
        /// </summary>
        public bool Category { get; set; }

        /// <summary>
        /// 提醒 0:無：1:提醒
        /// </summary>
        public bool Remind { get; set; }

        /// <summary>
        /// 狀態 1:預約中 ; 2:預約成功 ; 3:未出席 ; 4:已出席 ; 5:取消
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
