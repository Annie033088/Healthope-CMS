namespace PersistentLayer.Models
{
    public class EditPwdDto
    {
        /// <summary>
        /// 管理員Id
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 管理員舊密碼
        /// </summary>
        public string OldPwd { get; set; }

        /// <summary>
        /// 管理員新密碼
        /// </summary>
        public string NewPwd { get; set; }
    }
}
