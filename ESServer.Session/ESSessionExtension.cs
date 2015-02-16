using System;
using System.Collections.Generic;

using Owin;

namespace com.erinus.ESServer
{
    public static class ESSessionExtension
    {
        public static IAppBuilder UseSession(this IAppBuilder app)
        {
            return app.Use(typeof(ESSessionMiddleware), new ESSessionOptions { Store = ESSessionOptions.StoreType.Memory });
        }

        public static IAppBuilder UseSession(this IAppBuilder app, ESSessionOptions options)
        {
            return app.Use(typeof(ESSessionMiddleware), options);
        }

        public static ESSession AsSession(this IDictionary<String, Object> owinEnvironment)
        {
            return owinEnvironment[ESSessionMiddleware.StoreEnvironmentKey] as ESSession;
        }
    }
}