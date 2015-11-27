using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.DTOs;
using Contracts.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SchedulerServiceApp.Actors
{
    public interface INodeManagerActor
    {
        void Start();
        void Stop();
    }

    internal class WorkerNodeManagerActor : INodeManagerActor
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;

        public WorkerNodeManagerActor(IConfigurationService configurationService, ILogger logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }

        public void Start()
        {
            Connect();
            _logger.LogOperations("WorkerNodeManagerActor.Start");
        }

        public void Stop()
        {
            _connection.Close(1000);
            _logger.LogOperations("WorkerNodeManagerActor.Stop");
        }

        #region Helpers

        private void Connect()
        {
            var cf = new ConnectionFactory();
            _connection = cf.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_configurationService.WorkersExchange, ExchangeType.Direct);

            // Listen for heartbeats
            _channel.QueueDeclare(_configurationService.WorkerPingQueue, false, false, true, null);
            _channel.QueueBind(_configurationService.WorkerPingQueue, _configurationService.WorkersExchange, _configurationService.WorkerPingQueue);
            var heartbeatConsumer = new EventingBasicConsumer(_channel);
            heartbeatConsumer.Received += OnReceivedHeartbeat;
            heartbeatConsumer.ConsumerCancelled += OnConsumerCancelled;
            heartbeatConsumer.Shutdown += OnShutdown;
            _channel.BasicConsume(_configurationService.WorkerPingQueue, true, heartbeatConsumer);

            // Listen or status changes
            _channel.QueueDeclare(_configurationService.WorkerStatusQueue, false, false, true, null);
            _channel.QueueBind(_configurationService.WorkerStatusQueue, _configurationService.WorkersExchange, _configurationService.WorkerStatusQueue);
            var statusConsumer = new EventingBasicConsumer(_channel);
            statusConsumer.Received += OnReceivedStatus;
            statusConsumer.ConsumerCancelled += OnConsumerCancelled;
            statusConsumer.Shutdown += OnShutdown;
            _channel.BasicConsume(_configurationService.WorkerStatusQueue, false, statusConsumer);
        }

        private void OnReceivedHeartbeat(object sender, BasicDeliverEventArgs e)
        {
            var payload = Encoding.UTF8.GetString(e.Body);
            var nodeId = ulong.Parse(payload);
            UpdateWorkerKeepalive(nodeId);

            _logger.LogInformation(string.Format("WorkerNodeManagerActor received heartbeat from {0}", nodeId));
        }

        private void OnReceivedStatus(object sender, BasicDeliverEventArgs e)
        {
            var payload = Encoding.UTF8.GetString(e.Body);
            var nodeStatus = JsonConvert.DeserializeObject<WorkerNodeStatusDto>(payload);
            UpdateWorkerStatus(nodeStatus);

            _channel.BasicAck(e.DeliveryTag, false);

            _logger.LogInformation(string.Format("WorkerNodeManagerActor received status update from {0}", nodeStatus.NodeId));
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            //TODO
        }

        private void OnShutdown(object sender, ShutdownEventArgs e)
        {
            //TODO
        }

        private void UpdateWorkerKeepalive(ulong nodeId)
        {
        }

        private void UpdateWorkerStatus(WorkerNodeStatusDto nodeStatus)
        {
        }

        #endregion
    }
}
