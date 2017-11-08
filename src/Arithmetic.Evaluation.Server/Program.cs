using System;
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
            int port = Convert.ToInt32(configuration.GetSection("ServicePort").Value);
            SerilogExtensions.InitializeSerilog(configuration);

            ArithmeticEvaluationServer server = new ArithmeticEvaluationServer(IPAddress.Parse("127.0.0.1"), port);
            server.StartAsync().Wait();
        }
    }
}
