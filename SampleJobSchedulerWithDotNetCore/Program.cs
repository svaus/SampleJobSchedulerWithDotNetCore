using Autofac;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using Topshelf;
using Topshelf.Autofac;

namespace SampleJobSchedulerWithDotNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            IContainer container = DependencyInjection.Build();


            HostFactory.Run(hostConfigurator =>
            {
                // Set windows service properties
                hostConfigurator.SetServiceName("SampleSchedulerService");
                hostConfigurator.SetDisplayName("Sample Service");
                hostConfigurator.SetDescription("Executes Job.");

                hostConfigurator.RunAsLocalSystem();
                // Configure Log4Net with Topself
                hostConfigurator.UseLog4Net();
                hostConfigurator.UseAutofacContainer(container);
                hostConfigurator.Service<SchedulerService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(hostSettings => container.Resolve<SchedulerService>());
                    serviceConfigurator.WhenStarted(s => s.Start());
                    serviceConfigurator.WhenStopped(s => s.Shutdown());
                });
            });
        }
    }
}
