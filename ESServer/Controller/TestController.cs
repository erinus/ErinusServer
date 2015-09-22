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
    public class TestController : ESController
    {
        #region
        [Route("test")]
        [HttpGet]
        public HttpResponseMessage test()
        {
            try
            {
                ESSession session = this.Session(Request.GetOwinContext());

                if (session.Has("test"))
                {
                    Console.WriteLine("Session has [test] = " + session.Get<String>("test"));
                }
                else
                {
                    Console.WriteLine("Session has no key [test]");
                }

                if (!session.Has("test"))
                {
                    Console.WriteLine("Session set [test] = 123");

                    session.Set("test", "123");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.WriteLine(e.StackTrace);
            }

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
