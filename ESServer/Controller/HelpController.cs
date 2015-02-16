using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace com.erinus.ESServer.Controller
{
    public class HelpController : ESController
    {
        #region
        [Route("help")]
        [HttpGet]
        public HttpResponseMessage help()
        {
            //ESSession session = this.Session(Request.GetOwinContext());

            //if (session.Has("test"))
            //{
            //    Console.WriteLine("Get: " + session.Get<String>("test"));
            //}

            //Console.WriteLine("test");

            //if (!session.Has("test"))
            //{
            //    Console.WriteLine("Set");
            //    session.Set("test", "123");
            //}

            //String html = File.ReadAllText("test.htm.tpl");

            //ESRazorEngine.Set("test", html);

            //html = ESRazorEngine.Get("test", new { UserName = name });

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //response.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(html)));

            //response.Content = new StreamContent(new FileStream("test.htm.tpl", FileMode.Open));

            //return response;

            return Request.CreateResponse(HttpStatusCode.OK, new { name = "ErinusServer", code = "1.0.0.0" }, "application/json");
        }
        #endregion
    }
}
