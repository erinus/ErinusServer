using System;
using System.Collections.Generic;

namespace com.erinus.ESServer.Session.Store
{
    public class Memory : ISessionStore
    {
        public class Session : ISession
        {
            public DateTime Time;

            public String Key;

            public Dictionary<String, dynamic> Values;

            public Session(String sessionKey)
            {
                this.Time = DateTime.Now.ToLocalTime();

                this.Key = sessionKey;

                this.Values = new Dictionary<String, dynamic>();
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
                this.Values[key] = value;

                return true;
            }
        }

        private Dictionary<String, Session> sessions;

        public Memory()
        {
            this.sessions = new Dictionary<String, Session>();
        }

        public ISession Add(String sessionKey)
        {
            Session session = new Session(sessionKey);

            this.sessions.Add(sessionKey, session);

            return session;
        }

        public Boolean Has(String sessionKey)
        {
            return this.sessions.ContainsKey(sessionKey);
        }

        public ISession Get(String sessionKey)
        {
            return this.sessions[sessionKey];
        }
    }
}
