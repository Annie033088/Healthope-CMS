using System;

namespace ApiLayer.Models.MemberClass.Request
{
    public class RequestEditMemberPersonalClassStatusDto
    {
        /// <summary>
        /// 會員預約的私人課 Id
        /// </summary>
        public int MemberPersonalClassId { get; set; }

        /// <summary>
        /// 狀態 1:預約中 ; 2:預約成功 ; 3:未出席 ; 4:已出席 ; 5:取消
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}