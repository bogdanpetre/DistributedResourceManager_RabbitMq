using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Definitions;

namespace Contracts.DTOs
{
    public struct WorkerNodeStatusDto
    {
        public ulong NodeId { get; set; }   // InstanceId:16, WorkerType:16, IPv4:32
        public WorkerStatus Status { get; set; }
        public uint JobsCompleted { get; set; }
    }
}
