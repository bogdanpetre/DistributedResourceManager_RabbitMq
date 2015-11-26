using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Contracts.Definitions;
using Contracts.DTOs;
using Contracts.Interfaces;
using WorkerServiceApp.Actors;

namespace WorkerServiceApp
{
    class AppBootstrapper
    {
        public IContainer Container { get; private set; }

        public void Run()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>().SingleInstance();

            builder.RegisterType<WorkerServiceWrapper>().AsSelf().SingleInstance();
            builder.RegisterType<TimeSignalGenerator>().As<ISignalGenerator>();
            builder.RegisterType<WorkerActor>().As<IWorkerActor>();
            builder.RegisterType<WorkerStatusAgent>().As<IWorkerStatusAgent>()
                .WithParameter(new TypedParameter(typeof(WorkerNodeStatusDto), new WorkerNodeStatusDto
                {
                    NodeId =  1 << 48 | 1 << 32 | 1 << 0,
                    JobsCompleted = 0,
                    Status = WorkerStatus.Idle
                }));

            Container = builder.Build();
        }
    }
}
