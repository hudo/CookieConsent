﻿using System;
using Microsoft.Owin.Hosting;

namespace CookieConsent.Example.OWIN
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://+:8080";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
