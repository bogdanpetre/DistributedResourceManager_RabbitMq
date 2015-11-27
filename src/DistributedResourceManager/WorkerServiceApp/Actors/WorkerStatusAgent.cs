using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Definitions;
using Contracts.DTOs;
using Contracts.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace WorkerServiceApp.Actors
{
    public interface IWorkerStatusAgent
    {
        void Start();
        void Stop();
        void UpdateStatus(WorkerStatus newStatus, uint jobsCompleted);
    }

    internal class WorkerStatusAgent : IWorkerStatusAgent
    {
        private WorkerNodeStatusDto _nodeStatus;
        private readonly IConfigurationService _configurationService;
        private readonly ISignalGenerator _pingTimer;
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;

        public WorkerStatusAgent(WorkerNodeStatusDto nodeStatus, IConfigurationService configurationService, ISignalGenerator pingTimer, ILogger logger)
        {
            _nodeStatus = nodeStatus;
            _configurationService = configurationService;
            _pingTimer = pingTimer;
            _logger = logger;
        }

        public void Start()
        {
            Connect();
            _pingTimer.Initialize(_configurationService.WorkerStatusReportInterval);
            _pingTimer.Tick += SendPing;
            _pingTimer.Start();
        }

        public void Stop()
        {
            _pingTimer.Tick -= SendPing;
            _pingTimer.Stop();
            _connection.Close(1000);
        }

        public void UpdateStatus(WorkerStatus newStatus, uint jobsCompleted)
        {
            _nodeStatus.Status = newStatus;
            _nodeStatus.JobsCompleted = jobsCompleted;

            SendStatusUpdate();
        }

        #region Helpers

        private void Connect()
        {
            var cf = new ConnectionFactory();
            _connection = cf.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_configurationService.WorkersExchange, ExchangeType.Direct);

            _channel.QueueDeclare(_configurationService.WorkerStatusQueue, false, false, true, null);
            _channel.QueueBind(_configurationService.WorkerStatusQueue, _configurationService.WorkersExchange, string.Empty);

            _channel.QueueDeclare(_configurationService.WorkerPingQueue, false, false, true, null);
            _channel.QueueBind(_configurationService.WorkerPingQueue, _configurationService.WorkersExchange, string.Empty);

            _channel.ConfirmSelect();
        }

        private void SendPing()
        {
            var body = Encoding.UTF8.GetBytes(_nodeStatus.NodeId.ToString());
            _channel.BasicPublish(_configurationService.WorkersExchange, _configurationService.WorkerPingQueue, null, body);

            _logger.LogInformation(string.Format("WorkerStatusAgent sent ping for node {0}", _nodeStatus.NodeId));
        }

        private void SendStatusUpdate()
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_nodeStatus));
            _channel.BasicPublish(_configurationService.WorkersExchange, _configurationService.WorkerStatusQueue, null, body);

            _logger.LogInformation(string.Format("WorkerStatusAgent sent status update for node {0}", _nodeStatus.NodeId));
        }

        #endregion
    }
}
