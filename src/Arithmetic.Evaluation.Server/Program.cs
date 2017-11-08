using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arithmetic.Evaluation.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ArithmeticEvaluationServer server = new ArithmeticEvaluationServer(IPAddress.Parse("127.0.0.1"), 1337);
            server.StartAsync().Wait();
        }
    }
}
