using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace P2FixAnAppDotNetCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));

            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
