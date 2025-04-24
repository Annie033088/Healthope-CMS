using System;

namespace ApiLayer.Interface
{
    public interface IRedisService
    {
        /// <summary>
        /// 將值存入 redis，並設定過期時間
        /// </summary>
        void SetValue<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// 取得值
        /// </summary>
        T GetValue<T>(string key);

        /// <summary>
        /// 根據 key 刪除資料
        /// </summary>
        void DeleteKey(string key);

        /// <summary>
        /// 檢查 key 是否存在
        /// </summary>
        bool KeyExist(string key);
    }
}
