using System.Net;
using Microsoft.Extensions.Configuration;
using Arithmetic.Evaluation.Shared.Extensions;

namespace Arithmetic.Evaluation.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot configuration = ConfigurationRootFactory.Create();
            SerilogExtensions.InitializeSerilog(configuration);

            ArithmeticEvaluationServer server = new ArithmeticEvaluationServer(IPAddress.Parse("127.0.0.1"), 1337);
            server.StartAsync().Wait();
        }
    }
}
