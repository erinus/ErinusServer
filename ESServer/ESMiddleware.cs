using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace com.erinus.ESServer
{
    public class ESMiddleware : OwinMiddleware
    {
        public ESMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public override async Task Invoke(IOwinContext context)
        {
            Console.WriteLine("         {0,-6}   {1}", context.Request.Method, context.Request.Uri);

            await this.Next.Invoke(context);

            Console.WriteLine("{0,-6}   {1,-6}   {2}", context.Response.StatusCode, context.Request.Method, context.Request.Uri);
        }
    }
}
