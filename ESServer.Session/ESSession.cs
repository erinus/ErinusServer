using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace com.erinus.ESServer
{
    public class ESSession
    {
        public readonly String sessionKey;

        public ESSession(String sessionKey)
        {
            this.sessionKey = sessionKey;

            if (!ESSessionMiddleware.sessions.Has(sessionKey))
            {
                ESSessionMiddleware.sessions.Add(sessionKey);
            }
        }

        public Boolean Has(String key)
        {
            return ESSessionMiddleware.sessions.Get(sessionKey).Has(key);
        }

        public T Get<T>(String key) where T : class
        {
            T value = null;

            if (ESSessionMiddleware.sessions.Get(sessionKey).Has(key))
            {
                value = ESSessionMiddleware.sessions.Get(sessionKey).Get<T>(key);
            }

            return value;
        }

        public void Set(String key, dynamic value)
        {
            ESSessionMiddleware.sessions.Get(sessionKey).Set(key, value);
        }

        public static async Task<ESSession> Parse(IDictionary<String, Object> owinEnvironment)
        {
            return await Task.Run<ESSession>(new Func<ESSession>(delegate()
            {
                IDictionary<String, String[]> headers;

                headers = owinEnvironment["owin.RequestHeaders"] as IDictionary<String, String[]>;

                String sessionKey = null;

                String[] cookies;

                List<String> cookieJar;

                if (!headers.TryGetValue("Cookie", out cookies))
                {
                    cookies = new String[0];
                }

                cookieJar = new List<String>(cookies);

                if (cookieJar.Count != 0)
                {
                    foreach (String cookie in cookies)
                    {
                        foreach (String item in cookie.Split(';'))
                        {
                            String[] pair = item.Split('=');

                            if (pair.Length != 2)
                            {
                                continue;
                            }

                            if (pair[0].Equals("ESSESSION"))
                            {
                                sessionKey = pair[1];
                            }
                        }
                    }
                }

                if (!String.IsNullOrEmpty(sessionKey))
                {
                    return new ESSession(sessionKey);
                }

                sessionKey = Guid.NewGuid().ToString("N");

                headers = owinEnvironment["owin.ResponseHeaders"] as IDictionary<String, String[]>;

                if (!headers.TryGetValue("Set-Cookie", out cookies))
                {
                    cookies = new String[0];
                }

                cookieJar = new List<String>(cookies);

                cookieJar.Add(String.Format("ESSESSION={0};Expires={1};HttpOnly", sessionKey, DateTime.Now.AddDays(30).ToString("ddd, dd-MMM-yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture)));

                headers["Set-Cookie"] = cookieJar.ToArray();

                return new ESSession(sessionKey);
            }));
        }
    }
}
