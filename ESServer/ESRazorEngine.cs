using System;
using System.Collections.Generic;

using RazorEngine;
using RazorEngine.Templating;

namespace com.erinus.ESServer
{
    public class ESRazorEngine
    {
        public static void Set(String name, String template)
        {
            Engine.Razor.AddTemplate(name, template);

            Engine.Razor.Compile(name, null);
        }

        public static String Get(String name, object model)
        {
            return Engine.Razor.Run(name, null, model);
        }
    }
}
