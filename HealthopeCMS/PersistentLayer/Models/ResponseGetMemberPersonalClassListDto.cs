using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentLayer.Models
{
    public class ResponseGetMemberPersonalClassListDto
    {
        /// <summary>
        /// 課程列表
        /// </summary>
        public List<ResponseGetMemberPersonalClassDto> MemberPersonalClassList { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }

    public class ResponseGetMemberPersonalClassDto
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
        /// 會員手機
        /// </summary>
        public int MemberPhone { get; set; }

        /// <summary>
        /// 會員名稱
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 教練 Id
        /// </summary>
        public int CoachId { get; set; }

        /// <summary>
        /// 教練姓名
        /// </summary>
        public string CoachName { get; set; }

        /// <summary>
        /// 上課時間
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 課程分類 0:體驗課程 ; 1:付費課程
        /// </summary>
        public bool Category { get; set; }

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
