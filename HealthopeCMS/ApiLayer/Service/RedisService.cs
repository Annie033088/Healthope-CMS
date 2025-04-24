using System;
using ApiLayer.Interface;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ApiLayer.Service
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer redis;

        public RedisService(ConnectionMultiplexer redis)
        {
            this.redis = redis;
        }

        /// <summary>
        /// 將值存入 redis，並設定過期時間
        /// </summary>
        public void SetValue<T>(string key, T value, TimeSpan? expiry = null)
        {
            IDatabase db = redis.GetDatabase();

            // 將物件轉換為 JSON 字串
            string jsonValue = JsonConvert.SerializeObject(value);
            db.StringSet(key, jsonValue, expiry);
        }

        /// <summary>
        /// 取得值
        /// </summary>
        public T GetValue<T>(string key)
        {
            IDatabase db = redis.GetDatabase();

            string jsonValue = db.StringGet(key);
            if (jsonValue == null) return default(T);

            // 將 json 字串轉回物件
            return JsonConvert.DeserializeObject<T>(jsonValue);
        }

        /// <summary>
        /// 根據 key 刪除資料
        /// </summary>
        public void DeleteKey(string key)
        {
            IDatabase db = redis.GetDatabase();
            db.KeyDelete(key);
        }

        /// <summary>
        /// 檢查 key 是否存在
        /// </summary>
        public bool KeyExist(string key)
        {
            IDatabase db = redis.GetDatabase();
            return db.KeyExists(key);
        }
    }
}