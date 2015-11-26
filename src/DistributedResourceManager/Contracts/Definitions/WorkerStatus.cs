using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Definitions
{
    public enum WorkerStatus : byte
    {
        NotSet = 0,
        Idle,
        Busy
    }
}
