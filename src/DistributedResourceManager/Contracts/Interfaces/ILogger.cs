using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Interfaces
{
    public interface ILogger
    {
        void LogOperations(string message);
        void LogPerformance(string message);
        void LogInformation(string message);
    }

    public sealed class Logger : ILogger
    {
        public void LogInformation(string message)
        {
            Console.WriteLine("{0} - [INFO] {1}", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss.ffff"), message);
        }

        public void LogOperations(string message)
        {
            Console.WriteLine("{0} - [OPER] {1}", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss.ffff"), message);
        }

        public void LogPerformance(string message)
        {
            Console.WriteLine("{0} - [PERF] {1}", DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss.ffff"), message);
        }
    }
}
