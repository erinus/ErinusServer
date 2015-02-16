using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using com.erinus.ESServer.Session.Store;

namespace com.erinus.ESServer
{
    public class ESSessionMiddleware
    {
        public static String StoreEnvironmentKey = "esserver.session";

        public static ISessionStore sessions;

        readonly Func<IDictionary<String, Object>, Task> next;

        public ESSessionMiddleware(Func<IDictionary<String, Object>, Task> next)
            : this(next, new ESSessionOptions { Store = ESSessionOptions.StoreType.Memory })
        {

        }

        public ESSessionMiddleware(Func<IDictionary<String, Object>, Task> next, ESSessionOptions options)
        {
            this.next = next;

            String path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Assembly assembly;

            switch (options.Store)
            {
                case ESSessionOptions.StoreType.Memory:
                    path = String.Format(@"{0}\ESServer.Session.Store.Memory.dll", path);
                    if (!File.Exists(path))
                    {
                        Console.WriteLine("[FAILURE] ESServer.Session.Store.Memory.dll was not found. Session.Store.Memory disabled.");
                        break;
                    }
                    assembly = Assembly.LoadFile(path);
                    sessions = (ISessionStore)Activator.CreateInstance(assembly.GetType("com.erinus.ESServer.Session.Store.Memory"));
                    Console.WriteLine("[SUCCESS] Session.Store.Memory enabled.");
                    break;
                case ESSessionOptions.StoreType.MongoDB:
                    path = String.Format(@"{0}\ESServer.Session.Store.MongoDB.dll", path);
                    if (!File.Exists(path))
                    {
                        Console.WriteLine("[FAILURE] ESServer.Session.Store.MongoDB.dll was not found. Session.Store.MongoDB disabled.");
                        break;
                    }
                    assembly = Assembly.LoadFile(path);
                    sessions = (ISessionStore)Activator.CreateInstance(assembly.GetType("com.erinus.ESServer.Session.Store.MongoDB"));
                    Console.WriteLine("[SUCCESS] Session.Store.MongoDB enabled.");
                    break;
            }
        }

        public async Task Invoke(IDictionary<String, Object> environment)
        {
            ESSession session = await ESSession.Parse(environment);

            environment["esserver.session"] = session;

            await next(environment);
        }
    }
}
