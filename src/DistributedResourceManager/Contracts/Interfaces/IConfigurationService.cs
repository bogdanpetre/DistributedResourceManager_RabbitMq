using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface IConfigurationService
    {
        string WorkersExchange { get; }
        string WorkerStatusQueue { get; }
        TimeSpan WorkerStatusReportInterval { get; }
    }

    public sealed class ConfigurationService : IConfigurationService
    {
        public string WorkersExchange { get { return "X_WorkersExchange"; } }
        public string WorkerStatusQueue { get { return "X_WorkerStatuses"; } }
        public TimeSpan WorkerStatusReportInterval { get { return TimeSpan.FromMilliseconds(1000); } }
    }
}
