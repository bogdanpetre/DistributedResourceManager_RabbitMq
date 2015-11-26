using WorkerServiceApp.Actors;

namespace WorkerServiceApp
{
    internal class WorkerServiceWrapper
    {
        private readonly IWorkerActor _workerActor;
        private readonly IWorkerStatusAgent _statusAgent;

        public WorkerServiceWrapper(IWorkerActor workerActor, IWorkerStatusAgent statusAgent)
        {
            _workerActor = workerActor;
            _statusAgent = statusAgent;
        }

        public void Start()
        {
            _statusAgent.Start();
            _workerActor.Start();
        }

        public void Stop()
        {
            _workerActor.Stop();
            _statusAgent.Stop();
        }
    }
}