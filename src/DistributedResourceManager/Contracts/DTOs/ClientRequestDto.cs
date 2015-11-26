using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Interfaces;

namespace Contracts.DTOs
{
    public struct ClientRequestDto : IMessage
    {
        public Guid CorrelationId { get; set; }
    }
}
