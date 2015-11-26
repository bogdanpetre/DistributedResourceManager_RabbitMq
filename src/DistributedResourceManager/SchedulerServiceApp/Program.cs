using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Topshelf;

namespace SchedulerServiceApp
{
    class Program
    {
        static AppBootstrapper _bootstrapper;

        static void Main(string[] args)
        {
            HostFactory.Run(hc =>
            {
                hc.Service<SchedulerServiceWrapper>(sc =>
                {
                    sc.ConstructUsing(() =>
                    {
                        _bootstrapper = new AppBootstrapper();
                        _bootstrapper.Run();
                        return _bootstrapper.Container.Resolve<SchedulerServiceWrapper>();
                    });
                    sc.WhenStarted(s => s.Start());
                    sc.WhenStopped(s => s.Stop());

                });
                hc.SetServiceName("SchedulerServiceDemo");
                hc.SetDisplayName("SchedulerService Demo");
                hc.SetDescription("Demo of a distributed system comprised of a scheduler and many generic workers");
                hc.RunAsNetworkService();
            });
        }
    }
}
