using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Topshelf;

namespace WorkerServiceApp
{
    class Program
    {
        static AppBootstrapper _bootstrapper;

        static void Main(string[] args)
        {
            HostFactory.Run(hc =>
            {
                hc.Service<WorkerServiceWrapper>(sc =>
                {
                    sc.ConstructUsing(() =>
                    {
                        _bootstrapper = new AppBootstrapper();
                        _bootstrapper.Run();
                        return _bootstrapper.Container.Resolve<WorkerServiceWrapper>();
                    });
                    sc.WhenStarted(s => s.Start());
                    sc.WhenStopped(s => s.Stop());

                });
                hc.SetServiceName("Demo_WorkerService");
                hc.SetDisplayName("WorkerService Demo");
                hc.SetDescription("Demo Worker service");
                hc.RunAsNetworkService();
            });
        }
    }
}
