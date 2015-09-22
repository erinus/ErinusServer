using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace com.erinus.ESServer
{
	public class ESMediaTypeFormatter : MediaTypeFormatter
	{
		public ESMediaTypeFormatter()
		{
			this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/css"));
			this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));
			this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

		public override bool CanReadType(Type type)
		{
			return type == typeof(string);
		}

		public override bool CanWriteType(Type type)
		{
			return false;
		}
	}
}
