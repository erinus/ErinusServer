using System.IO;
using System.Net;
using System.Web.Http;
using System.Web.Http.Dispatcher;

using Owin;

using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
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

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;

            config.MapHttpAttributeRoutes();

            config.Services.Replace(typeof(IAssembliesResolver), new ESAssembliesResolver());

            DirectoryInfo dire = new DirectoryInfo(@".\web");

            if (dire.Exists)
            {
                FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();

                provider.Mappings.Add(".iso", "application/iso");

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileSystem = new PhysicalFileSystem(dire.FullName),

                    RequestPath = new PathString("/web"),

                    ContentTypeProvider = provider
                });
            }

            app.Use<ESMiddleware>();

            app.UseSession(new ESSessionOptions
            {
                Store = ESSessionOptions.StoreType.Redis
            });

            app.UseWebApi(config);
        }
    }
}
