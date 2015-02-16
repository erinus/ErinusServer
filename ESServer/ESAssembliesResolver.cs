using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace com.erinus.ESServer
{
    public class ESAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            List<Assembly> result = new List<Assembly>(base.GetAssemblies());

            DirectoryInfo dire = new DirectoryInfo(@".\controllers");

            if (dire.Exists)
            {
                foreach (FileInfo file in dire.GetFiles("*.dll"))
                {
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(file.FullName));

                    result.Add(assembly);
                }
            }

            return result;
        }
    }
}
