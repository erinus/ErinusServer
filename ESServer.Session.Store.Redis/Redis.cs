using System;
using System.Collections.Generic;

namespace com.erinus.ESServer.Session.Store
{
    public class Redis : ISessionStore
    {
        public class Session : ISession
        {
            public DateTime Time;
            public Dictionary<String, dynamic> Values;

            public Boolean Has(String key)
            {
                return true;
            }

            public T Get<T>(String key) where T : class
            {
                T value = null;

                return value;
            }

            public Boolean Set(String key, dynamic value)
            {
                return true;
            }
        }

        public Redis()
        {

        }

        ~Redis()
        {

        }

        public ISession Add(String sessionKey)
        {
            Session session = new Session();



            return session;
        }

        public Boolean Has(String sessionKey)
        {
            return true;
        }

        public ISession Get(String sessionKey)
        {
            return null;
        }
    }
}
