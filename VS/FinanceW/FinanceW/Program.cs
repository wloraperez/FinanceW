﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FinanceW
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            //var host = new WebHostBuilder() 
            //.UseKestrel()
            //.UseContentRoot(Directory.GetCurrentDirectory())
            //.UseIISIntegration()
            //.UseStartup<Startup>()
            //.Build();

            //host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
