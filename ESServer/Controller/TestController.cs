using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			//String html = File.ReadAllText("test.htm.tpl");

			//ESRazorEngine.Set("test", html);

			//html = ESRazorEngine.Get("test", new { UserName = name });

			//HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

			//response.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(html)));

			//response.Content = new StreamContent(new FileStream("test.htm.tpl", FileMode.Open));

			//return response;

			return Request.CreateResponse(HttpStatusCode.OK);
		}
		#endregion

		#region
		[Route("test/session/{key}")]
		[HttpGet]
		public HttpResponseMessage test_session(String key)
		{
			try
			{
				ESSession session = this.Session(Request.GetOwinContext());

				if (session.Has(key))
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { val = session.Get<String>(key) }, "application/json");
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { val = "none" }, "application/json");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

				Console.WriteLine(e.StackTrace);

				return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = e.Message, stacktrace = e.StackTrace }, "application/json");
			}
		}
		#endregion

		#region
		[Route("test/session/{key}/{val}")]
		[HttpGet]
		public HttpResponseMessage test_session(String key, String val)
		{
			try
			{
				ESSession session = this.Session(Request.GetOwinContext());

				session.Set(key, val);

				return Request.CreateResponse(HttpStatusCode.OK, new { key = key, val = val }, "application/json");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

				Console.WriteLine(e.StackTrace);

				return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = e.Message, stacktrace = e.StackTrace }, "application/json");
			}
		}
		#endregion
    }
}
