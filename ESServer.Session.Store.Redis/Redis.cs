using System;
using System.Collections.Generic;

using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace com.erinus.ESServer.Session.Store
{
    public class Redis : ISessionStore
    {
        public class _Session
        {
            [JsonProperty("Time")]
            public DateTime Time;

            [JsonProperty("Values")]
            public Dictionary<String, dynamic> Values;
        }

        public class Session : ISession
        {
            public DateTime Time;

            public String Key;

            public Dictionary<String, dynamic> Values;

            private IRedisClient Client;

            public Session(IRedisClient client, String sessionKey)
            {
                this.Time = DateTime.Now.ToLocalTime();

                this.Key = sessionKey;

                this.Values = new Dictionary<String, dynamic>();

                this.Client = client;
            }

            public Boolean Has(String key)
            {
                return this.Values.ContainsKey(key);
            }

            public T Get<T>(String key) where T : class
            {
                T value = null;

                if (this.Values.ContainsKey(key))
                {
                    value = this.Values[key] as T;
                }

                return value;
            }

            public Boolean Set(String key, dynamic value)
            {
                this.Values.Add(key, value);

                this.Client.Set<String>(

                    this.Key,

                    JsonConvert.SerializeObject(

                        new _Session
                        {
                            Time = DateTime.Now.ToLocalTime(),

                            Values = this.Values
                        }
                    )

                );

                return true;
            }
        }

        private IRedisClientsManager redisClientManager;

        private IRedisClient redisClient;

        public Redis()
        {
            this.redisClientManager = new PooledRedisClientManager();

            this.redisClient = this.redisClientManager.GetClient();
        }

        ~Redis()
        {
            if (this.redisClient != null)
            {
                this.redisClient.Dispose();
            }
        }

        public ISession Add(String sessionKey)
        {
            this.redisClient.Set<String>(

                sessionKey,

                JsonConvert.SerializeObject(

                    new _Session
                    {
                        Time = DateTime.Now.ToLocalTime(),

                        Values = new Dictionary<String, dynamic>()
                    }
                )

            );

            Session session = new Session(this.redisClient, sessionKey);

            return session;
        }

        public Boolean Has(String sessionKey)
        {
            return this.redisClient.ContainsKey(sessionKey);
        }

        public ISession Get(String sessionKey)
        {
            _Session _session = JsonConvert.DeserializeObject<_Session>(this.redisClient.Get<String>(sessionKey));

            Session session = new Session(this.redisClient, sessionKey);

            foreach (KeyValuePair<String, dynamic> pair in _session.Values)
            {
                session.Values.Add(pair.Key, pair.Value);
            }

            return session;
        }
    }
}
