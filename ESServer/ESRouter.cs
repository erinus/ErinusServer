using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Web.Http.Dispatcher;

using Owin;

using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.ContentTypes;

namespace com.erinus.ESServer
{
    public class ESRouter
    {
        public void Configuration(IAppBuilder app)
        {
            HttpListener listener = (HttpListener)app.Properties["System.Net.HttpListener"];

            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Services.Replace(typeof(IAssembliesResolver), new ESAssembliesResolver());

            DirectoryInfo dire = new DirectoryInfo(@".\websites");

            if (dire.Exists)
            {
                FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();

                provider.Mappings.Add(".iso", "application/iso");

                StaticFileOptions option = new StaticFileOptions
                {
                    FileSystem = new PhysicalFileSystem(dire.FullName),
                    RequestPath = new PathString("/web"),
                    ContentTypeProvider = provider
                };

                app.UseStaticFiles(option);
            }

            app.Use<ESMiddleware>();

            app.UseSession(new ESSessionOptions
            {
                Store = ESSessionOptions.StoreType.Memory
            });

            app.UseWebApi(config);
        }
    }
}
