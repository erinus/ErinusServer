using System;
using System.Net;

using Microsoft.Owin.Hosting;

namespace com.erinus.ESServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<ESRouter>("http://+:11111"))
            {
                Console.WriteLine(String.Format("ErinusServer {0}-{1}", "1.0.0.0", "20150213 "));

                while (Console.ReadKey(true).Key != ConsoleKey.Escape)
                {

                }
            }
        }
    }
}
