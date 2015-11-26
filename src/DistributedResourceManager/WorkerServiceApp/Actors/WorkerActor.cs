using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceApp.Actors
{
    public interface IWorkerActor
    {
        void Start();
        void Stop();
    }

    internal class WorkerActor : IWorkerActor
    {
        private readonly IWorkerStatusAgent _workerStatusAgent;

        public WorkerActor(IWorkerStatusAgent workerStatusAgent)
        {
            _workerStatusAgent = workerStatusAgent;
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
