using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Contracts.Interfaces;
using SchedulerServiceApp.Actors;

namespace SchedulerServiceApp
{
    class AppBootstrapper
    {
        public IContainer Container { get; private set; }

        public void Run()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>().SingleInstance();

            builder.RegisterType<SchedulerServiceWrapper>().AsSelf();
            builder.RegisterType<WorkerNodeManagerActor>().As<INodeManagerActor>();

            Container = builder.Build();
        }
    }
}
