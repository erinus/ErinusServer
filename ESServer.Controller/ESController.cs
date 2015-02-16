using System;
using System.Web.Http;

using Microsoft.Owin;

namespace com.erinus.ESServer.Controller
{
    public class ESController : ApiController
    {
        public ESSession Session(IOwinContext context)
        {
            return context.Environment.AsSession();
        }
    }
}
