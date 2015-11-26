using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchedulerServiceApp.Actors;

namespace SchedulerServiceApp
{
    internal class SchedulerServiceWrapper
    {
        private readonly INodeManagerActor _nodeManager;
        public SchedulerServiceWrapper(INodeManagerActor nodeManager)
        {
            _nodeManager = nodeManager;
        }

        public void Start()
        {
            _nodeManager.Start();
        }

        public void Stop()
        {
            _nodeManager.Stop();
        }
    }
}
