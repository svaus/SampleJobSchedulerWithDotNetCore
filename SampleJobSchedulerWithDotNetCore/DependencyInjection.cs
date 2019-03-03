using Autofac;
using Autofac.Extras.Quartz;
using log4net;
using System;
using System.Collections.Specialized;

namespace SampleJobSchedulerWithDotNetCore
{
    public class DependencyInjection
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            // Registers SchedulerService with Autofac
            builder.RegisterType<SchedulerService>().AsSelf().InstancePerLifetimeScope();

            NameValueCollection props = new NameValueCollection
            {
                { "quartz.scheduler.instanceName", "SampleJobScheduler" },                
                { "quartz.threadPool.threadCount" , "5" },
                { "quartz.threadPool.threadPriority", "Normal" },
                { "quartz.plugin.xml.type", "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins" },
                { "quartz.plugin.xml.fileNames" , "~/quartzjobconfig.xml" }

            };

            // Register Quartz with Autofac and reads quartz section in App.config. 
            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = context => props
                //ConfigurationProvider = context => (NameValueCollection)ConfigurationManager.GetSection("quartz")
            });

            // This line registers all Jobs in the current executing assembly
            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(SampleJob).Assembly));

            // As per recent update GetLogger might have some performance issues. 
            builder.Register(c => LogManager.GetLogger(typeof(Object))).As<ILog>();

            return builder.Build();
        }
    }
}
